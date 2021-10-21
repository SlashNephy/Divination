import json
import os
import urllib.error
import urllib.request
from zipfile import ZipFile

PROVIDER = os.getenv("PROVIDER", "dl.horoscope.dev")
USER_AGENT = os.getenv("USER_AGENT", "Dalamud.DivinationPluginRepo (+https://github.com/horoscope-dev/Dalamud.DivinationPluginRepo)")
SOURCE = os.getenv("SOURCE")
DALAMUD_ENV = os.environ["DALAMUD_ENV"]

def extract_manifests():
    manifests = []
    for dirpath, _, filenames in os.walk(f"dist/{DALAMUD_ENV}"):
        if "latest.zip" not in filenames:
            continue

        with ZipFile(f"{dirpath}/latest.zip") as z:
            plugin_name = dirpath.split("/")[-1]
            manifest = json.loads(z.read(f"{plugin_name}.json").decode())
            manifests.append(manifest)

    return manifests

def get_download_stats():
    try:
        request = urllib.request.Request(f"https://{PROVIDER}/statistics", headers={"User-Agent": USER_AGENT})
        with urllib.request.urlopen(request) as response:
            return json.load(response)
    except urllib.error.HTTPError:
        return {}
    except urllib.error.URLError:
        return {}

def add_extra_fields(manifests, downloads):
    for manifest in manifests:
        latest_zip = f"dist/{DALAMUD_ENV}/{manifest['InternalName']}/latest.zip"
        query = f"?source={SOURCE}" if SOURCE else ""

        manifest["IsTestingExclusive"] = DALAMUD_ENV == "testing"
        manifest["DownloadCount"] = downloads.get(manifest["InternalName"], 0)
        manifest["LastUpdated"] = int(os.path.getmtime(latest_zip))
        manifest["DownloadLinkInstall"] = manifest["DownloadLinkUpdate"] = f"https://{PROVIDER}/{DALAMUD_ENV}/{manifest['InternalName']}{query}"
        manifest["DownloadLinkTesting"] = f"https://{PROVIDER}/testing/{manifest['InternalName']}{query}"

def dump_master(manifests):
    manifests.sort(key=lambda x: x["InternalName"])

    with open(f"dist/{DALAMUD_ENV}/pluginmaster.json", "w") as f:
        json.dump(manifests, f, indent=2, sort_keys=True)

def generate_markdown(manifests, downloads):
    lines = [
        "# Divination Plugins",
        "",
        "| Name | Version | Author | Description | Total Downloads |",
        "|------|---------|--------|-------------|-----------------|"
    ]

    for manifest in manifests:
        name = f"[{manifest['Name']}]({manifest['RepoUrl']}) [💾]({manifest['DownloadLinkInstall']})"
        version = manifest["AssemblyVersion"]
        author = manifest["Author"]

        tags = [fr"\#{x}" for x in manifest.get("CategoryTags", []) + manifest.get("Tags", [])]
        description = f"{manifest.get('Punchline', '')}<br>{manifest.get('Description', '')}<br>{' '.join(tags)}"

        total_downloads = downloads.get(manifest["InternalName"], "n/a")

        lines.append(f"| {name} | {version} | {author} | {description} | {total_downloads} |")

    with open(f"plugins_{DALAMUD_ENV}.md", "w") as f:
        f.write("\n".join(lines))


if __name__ == "__main__":
    manifests = extract_manifests()
    downloads = get_download_stats()

    add_extra_fields(manifests, downloads)
    dump_master(manifests)

    generate_markdown(manifests, downloads)

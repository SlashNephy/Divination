import json
import os
from zipfile import ZipFile

DOWNLOAD_BASE_URI = os.environ["DOWNLOAD_BASE_URI"]
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

def add_extra_fields(manifests):
    for manifest in manifests:
        latest_zip = f"dist/{DALAMUD_ENV}/{manifest['InternalName']}/latest.zip"

        manifest["IsTestingExclusive"] = DALAMUD_ENV == "testing"
        manifest["DownloadCount"] = 0
        manifest["LastUpdated"] = int(os.path.getmtime(latest_zip))
        manifest["DownloadLinkInstall"] = manifest["DownloadLinkUpdate"] = f"{DOWNLOAD_BASE_URI}/{latest_zip}"
        manifest["DownloadLinkTesting"] = f"{DOWNLOAD_BASE_URI}/dist/testing/{manifest['InternalName']}/latest.zip"

def dump_master(manifests):
    manifests.sort(key=lambda x: x["InternalName"])

    with open(f"dist/{DALAMUD_ENV}/pluginmaster.json", "w") as f:
        json.dump(manifests, f, indent=2, sort_keys=True)


if __name__ == "__main__":
    manifests = extract_manifests()
    add_extra_fields(manifests)
    dump_master(manifests)

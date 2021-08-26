import json
import os
from os.path import getmtime
from time import time
from zipfile import ZIP_DEFLATED, ZipFile


DALAMUD_ENV = os.environ["DALAMUD_ENV"]

def extract_manifests():
    manifests = []
    for dirpath, _, filenames in os.walk(f"dist/{DALAMUD_ENV}"):
        if len(filenames) == 0 or "latest.zip" not in filenames:
            continue

        with ZipFile(f"{dirpath}/latest.zip") as z:
            plugin_name = dirpath.split("/")[-1]
            manifest = json.loads(z.read(f"{plugin_name}.json").decode())
            manifests.append(manifest)

    return manifests

def add_extra_fields(manifests):
    DEFAULTS = {
        "IsHide": False,
        "IsTestingExclusive": False,
        "ApplicableVersion": "any",
        "DownloadCount": 0
    }

    for manifest in manifests:
        download_url = "https://raw.githubusercontent.com/SlashNephy/Dalamud.DivinationPluginRepo/master/dist/{env}/{name}/latest.zip"

        manifest["DownloadLinkInstall"] = manifest["DownloadLinkTesting"] = manifest["DownloadLinkUpdate"] = download_url.format(
            env=DALAMUD_ENV,
            name=manifest["InternalName"]
        )

        for k, v in DEFAULTS.items():
            if k not in manifest:
                manifest[k] = v

def update_last_updated(manifests):
    for manifest in manifests:
        latest = f"dist/{DALAMUD_ENV}/{manifest['InternalName']}/latest.zip"
        modified = int(getmtime(latest))

        if "LastUpdated" not in manifest or modified != int(manifest["LastUpdated"]):
            manifest["LastUpdated"] = str(modified)

def dump_master(manifests):
    with open("pluginmaster.json", "w") as f:
        json.dump(manifests, f, indent=4)

if __name__ == "__main__":
    manifests = extract_manifests()

    add_extra_fields(manifests)
    update_last_updated(manifests)

    dump_master(manifests)

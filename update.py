import json
import os
from os.path import getmtime
from zipfile import ZipFile


DOWNLOAD_BASE_URI = os.environ["DOWNLOAD_BASE_URI"]
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
        manifest["DownloadLinkInstall"] = manifest["DownloadLinkTesting"] = manifest["DownloadLinkUpdate"] = f"{DOWNLOAD_BASE_URI}/dist/{DALAMUD_ENV}/{manifest['InternalName']}/latest.zip"

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
    with open(f"dist/{DALAMUD_ENV}/pluginmaster.json", "w") as f:
        json.dump(manifests, f, indent=4)


if __name__ == "__main__":
    manifests = extract_manifests()

    add_extra_fields(manifests)
    update_last_updated(manifests)

    dump_master(manifests)

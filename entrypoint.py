import json
import os
import threading
import urllib.request
from collections import defaultdict

from flask import Flask, jsonify, redirect, request

CACHE_PATH = os.getenv("CACHE_PATH", "cache.json")
DEFAULT_SOURCE = os.getenv("DEFAULT_SOURCE", "repo.horoscope.dev")
USER_AGENT = os.getenv("USER_AGENT", "Dalamud.DivinationPluginRepo.DownloadCounter/1.0 (+https://github.com/horoscope-dev/Dalamud.DivinationPluginRepo.DownloadCounter)")

cache = defaultdict(lambda: 0)
cache_lock = threading.Lock()
app = Flask(__name__)

def check_if_exists(url):
    try:
        req = urllib.request.Request(url, method="HEAD", headers={"User-Agent": USER_AGENT})
        with urllib.request.urlopen(req):
            pass
    except urllib.error.HTTPError:
        return False

@app.route("/<any(stable, testing):channel>/<plugin>")
def download(channel, plugin):
    source = request.args.get("source", DEFAULT_SOURCE)
    url = f"https://{source}/dist/{channel}/{plugin}/latest.zip"

    if not check_if_exists(url):
        return

    with cache_lock:
        cache[plugin] += 1
        save_cache()

    return redirect(url)

@app.route("/statistics")
def statistics():
    with cache_lock:
        return jsonify(cache)

@app.route("/")
def index():
    return redirect(f"https://{DEFAULT_SOURCE}")

def load_cache():
    if not os.path.exists(CACHE_PATH):
        return

    with open(CACHE_PATH) as f:
        cache.update(json.load(f))

def save_cache():
    with open(CACHE_PATH, "w") as f:
        json.dump(cache, f, indent=2, sort_keys=True)


if __name__ == "__main__":
    load_cache()

    app.run()

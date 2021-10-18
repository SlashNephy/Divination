import os
import re

WORKFLOW_FILE = os.getenv("WORKFLOW_FILE")
TEMPLATE_FILE = os.getenv("TEMPLATE_FILE")

MARKER = "### BEGIN TEMPLATE ###"
PATTERN = re.compile(f"{MARKER}.*$", flags=re.MULTILINE | re.DOTALL)

def main():
  with open(WORKFLOW_FILE) as f:
    workflow = f.read()

  if not PATTERN.search(workflow):
    print(f"Target workflow file does not contain \"{MARKER}\". Skipping...")
    return

  with open(TEMPLATE_FILE) as f:
    template = PATTERN.search(f.read()).group(0)

  new_workflow = PATTERN.sub(template, workflow)
  with open(WORKFLOW_FILE, "w") as f:
    f.write(new_workflow)

if __name__ == "__main__":
  main()

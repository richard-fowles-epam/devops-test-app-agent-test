---
name: Issue Hygiene
description: Reviews open issues daily and flags any that do not follow the agent-task contract standard.
on:
  schedule:
    - cron: "0 9 * * *"
  workflow_dispatch:
permissions:
  contents: read
  issues: read
  pull-requests: read
  copilot-requests: write
engine: copilot
safe-outputs:
  add-labels:
    allowed: ["NEEDS REVIEW"]
    max: 10
---

# Issue Hygiene Review

Review every open issue in this repository against the task-contract standard defined by `.github/ISSUE_TEMPLATE/agent-task.yml`. A compliant issue must:

1. Have a title starting with `[Agent Task]: `.
2. Carry the `agent-task` label.
3. Contain all three required sections from the template, each with real, non-empty content (not just the placeholder text):
   - **1. Context and Inputs** — background, constraints, and starting point.
   - **2. Expected Outputs** — plan, pull request, and evidence expectations.
   - **3. Success Criteria** — the behaviour and checks that decide whether the task is done.

For each open issue:

- If it already carries the `NEEDS REVIEW` label, skip it — it has already been flagged.
- If it meets all three criteria above, leave it alone.
- If it is missing the title prefix, missing the `agent-task` label, missing one or more of the three sections, or any section is empty or clearly a placeholder, apply the `NEEDS REVIEW` label to it.

Do not edit issue titles, bodies, or any other labels. Do not comment on the issues. The only action to take is adding the `NEEDS REVIEW` label where warranted, via the `add-labels` safe output.

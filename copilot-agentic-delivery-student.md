# Agentic Engineering on GitHub

# Prerequisites

Complete every item below **before** day one. The first lesson validates this setup rather than establishing it, so anything missing here becomes a blocker once the course starts. Work through it as a checklist.

## Accounts and licensing

- [ ] A GitHub account.
- [ ] Any **paid GitHub Copilot plan** (Pro, Pro+, Business, Max, or Enterprise).
- [ ] **GitHub Actions enabled** for your working repository.

## Local software

- [ ] **Git.**
- [ ] **.NET SDK 10 or later.**
- [ ] **Visual Studio Code.**
- [ ] The **GitHub Copilot app**, installed and signed in.
- [ ] The **`gh` CLI**, authenticated.
- [ ] **PowerShell** (`pwsh`). The training repository's setup script is a PowerShell script (`scripts/seed-issues.ps1`); have it installed and on your `PATH` before the exercise that runs it.

## Repository preparation (mandatory, pre-course)

- [ ] **Clone** the training repository: [`epam-agent-forge/copilot-training-customer-management-app`](https://github.com/epam-agent-forge/copilot-training-customer-management-app).
- [ ] From that codebase, **create a new repository of your own** in the licensed account or organisation, and push the codebase to it. This is the repository you will build up throughout the course.
- [ ] In this repository, you must have **permissions to open issues, push branches, and raise pull requests**.
- [ ] Your working repository must **not** be owned by a managed user account, and must **not** have the cloud agent explicitly disabled.
- [ ] **Do not fork.** Later exercises create and manage issues in your repository, and a fork is not the intended model. Start from a new repository you own.
- [ ] **Protect the `main` branch.** Add a branch protection rule or ruleset on `main` before the course begins, so agent and student work alike goes through pull requests rather than direct pushes.

## Copilot app setup (mandatory, pre-course)

- [ ] **Open your working repository in the GitHub Copilot app.**
- [ ] **Start a new session for the repository.** Confirm the session uses a **new git worktree** rather than reusing an existing one, so exercise sessions do not collide with one another's branch state.

# Lesson 0 — Course Readiness

## Scenario

Welcome to this course on agentic engineering in GitHub.

Many businesses are experimenting with agentic technologies, but most are focusing primarily on coding agents. Coding agents are only one part of the solution. Without making use of the wider agentic ecosystem, businesses often struggle to progress beyond vibe coding.

GitHub provides a range of agentic capabilities, alongside the technologies that underpin modern software development and are increasingly essential to agentic engineering. Throughout this course, we will explore these through a series of hands-on exercises built around a simple codebase.

Learning the latest AI technologies can feel overwhelming, so each exercise introduces one concept at a time. This allows you to become comfortable with each tool or capability before moving on to the next.

The application we will use is a simple .NET Customer Management app, only slightly more complex than a Hello World example. Do not worry if you are unfamiliar with .NET. We will not explore the code in significant depth, because the focus of this course is not learning .NET, but experimenting with the agentic technologies available in GitHub.


## Concept — Agents Propose, You Accept

## Primer

Agents propose, and humans and policy accept. An agent can prepare a change, but whether that change takes effect is a decision the platform enforces rather than one the agent makes for itself.

How much an agent may do on its own is best set in tiers, matched to how much damage an action could do and how easily it could be undone. A read-only tier inspects and recommends but changes nothing. A propose-only tier opens branches and pull requests but cannot merge or deploy. An execute-with-guardrails tier may run pre-approved workflows within set limits. A human-authorised tier reserves high-impact actions, such as a production deploy, for explicit sign-off. The riskier and less reversible the action, the higher the tier it belongs in.

You enforce these tiers with ordinary platform controls rather than anything agent-specific: branch protection and rulesets, required status checks, CODEOWNERS on sensitive paths, and protected environments for deploys. The same machinery that governs any contributor governs an agent.

Judge the output the same way too. Weigh an agent's pull request on its merits, whether it solves the problem, whether the scope is right, whether the checks pass, without rejecting it merely because a machine wrote it or waving it through merely because automation produced it.


## Exercise

# Exercise: Validate course readiness

This exercise confirms that the platform capabilities and permissions the course depends on are available to you, and fixes any gaps before you delegate real work. It is a validation pass only: you confirm the repository and your access, you do not yet explore what is inside the repository.

**Exercise:**

1. **Confirm your repository's origin.** Check that the repository you will use for the course was **created from the training codebase and is a standalone repository, not a fork**.

2. **Confirm GitHub.com access.** Sign in and confirm you can reach the repository on GitHub.com and browse it in the web interface. Every delegation in this course is driven from the platform, so web access is the baseline.

3. **Confirm your repository permissions.** Confirm you can **open an issue, create a branch, and open a pull request** in the repository.

4. **Confirm the training app is open in the Copilot app.** The application should be cloned down and open in a session in the Copilot app, using a new worktree.

5. **Resolve any setup problems now.** For each check that fails, fix it before continuing: recreate the repository from the codebase if it turned out to be a fork, sort out licence or access gaps, and correct the remote connection. End the exercise with every check passing.

By the end you have a **validated, course-ready repository**: created from the codebase (not a fork), reachable on GitHub.com, covered by a Copilot licence for the cloud agent, code review, and agentic workflows, with issue, branch, and pull-request permissions confirmed and the local clone connected to its remote.

- The learning target is a **clean starting line**, not any application knowledge. Success is every readiness check passing; students should be able to name each capability the course leans on and confirm they have it.
- **Scope boundary (state it explicitly to students):**
  - *Before the course* (prerequisites, already done): cloning the codebase, creating their own repository from it, and installing the local tooling.
  - *This lesson*: validating all of that, capability by capability, and fixing gaps.
  - *Deferred to the first delegation*: any exploration of the application itself, its endpoints, or its tests. Do not let this exercise drift into a code tour; understanding the codebase is the explicit job of the first delegation exercise, where the agent gives the repository tour.
- The most common trap is a **fork**. A student who forked rather than created from the codebase will hit confusing permission and Actions behaviour later; catch it here.
- Step 3 is worth slowing down on: licence coverage for the cloud agent, code review, and agentic workflows is not uniform across plans, so confirm each rather than assuming one implies the others.

# Lesson 1 — Your First Delegation

## Scenario

The Customer Management application is new to us, so before we can add anything to it we need to understand what is already there: how the project is arranged, where the API is defined, and how it is tested. Once we know that, we can start building.

There is a good deal to build. The application is very small and has a single endpoint, `POST /customers`, which creates a customer. Nothing reads one back, and nothing updates or deletes one. The first gap we will close is retrieval, by adding `GET /customers/{id}`.

GitHub lets us do both parts of this on the repository directly, using agents. Let us look at how that works.


## Concept — GitHub Copilot Cloud Agent

## Primer

When you want a change made but do not want to write it yourself, hand the task to a cloud agent. It works autonomously in the background on GitHub, exploring the repository, planning, editing code, running the tests, and opening a pull request, all inside an ephemeral environment created for that run. You start it from wherever the task already lives: assign an issue to Copilot, mention `@copilot` on a pull request, or launch it from the Agents page.

Not every session has to end in code. From the Agents panel on GitHub.com you can also use it before building anything. **Explain repository** gives a read-only tour of an unfamiliar codebase. **Create a plan** drafts a step-by-step implementation plan you can review and refine with the agent before it writes any code. A session opened this way does not raise a pull request on its own; you get one only when you ask or click **Create pull request**.

However you start it, the agent takes one repository and produces one branch and one pull request per task, which keeps each delegation reviewable. It is propose-only by default: it can open a pull request but cannot merge it or push to your default branch, and the checks on its pull request wait until someone with write access approves them. The agent proposes; you approve, review, and merge.


## Concept — Agent Session Management

## Primer

Delegating to the cloud agent does not end once you have handed over the task. A running agent is a live session you can watch and steer while it works.

While the agent works, its session streams live in the Agents UI. You see its plan, the step it is on right now, and the tool calls it makes as they happen. You can watch it reason through your repo rather than wait on an opaque PR. More than one session can run at once.

If it starts down the wrong path, you can steer it instead of stopping and starting over. Send a plain-language message mid-session, such as "also update the tests" or "don't touch the config", and it adjusts course without restarting. Each steering message consumes AI credits, so steer deliberately rather than sending frequent, low-value messages.

When you would rather work on it directly, you can continue the session locally in the Copilot app, and it carries its context across so you resume where the cloud session left off. Because sessions are searchable, you can also query your history in natural language later. When it finishes, it hands you a pull request and you are back on the familiar review-and-merge path.


## Exercise

# Exercise: Delegate your first fix

You delegate your first change to the cloud agent rather than writing it yourself, and you guide it to a merged pull request. The goal is the full loop with you in control at every turn: **let the agent explain the repository, review its plan, steer its session, then review, approve, and merge its pull request**. You practise directing an agent's work without doing that work yourself, and you meet the codebase through the agent's own tour of it rather than by reading it cold.

**Exercise:**

1. **Get a tour of the repository.** In the **Agents** area, run **Explain repository** against your repository and read the explanation it produces. This is your repository tour: how the code is laid out, where the API is defined, and where the tests live.

2. **Ask the agent to plan the change.** Start a new session and use **Create a plan** to have the agent draft a step-by-step plan for adding a **GET customer endpoint** that retrieves a single customer by its identifier. Do not let it write code yet.

3. **Review the plan.** Read the plan and check it covers the whole change, not just the endpoint: note in particular that it proposes **test coverage, including acceptance tests**, alongside the code.

4. **Proceed to implementation, and steer as it works.** Let the agent implement the approved plan. Follow the session, its plan, current step, and tool calls, as it works, and send a short natural-language steering message if it drifts.

5. **Create the pull request.** Have the session **create a pull request** with its change. The agent proposes; it does not merge.

6. **Review, approve the run, and merge.** Read the diff against the four deliverables above. Because an agent's pull request has its **Actions runs held pending approval**, approve the workflow run, then confirm the `build-and-test` job goes green in the **Checks** tab. With the checks green and the review done, **merge the pull request**.

By the end the API can both create and retrieve a customer, and your repository holds the **GET customer endpoint** and its tests, merged: `GET /customers/{id}`, `GetCustomerTests.cs`, `GetCustomer.feature` with steps, and the corrected Swagger description.

- The learning target is the **control loop plus the agent's pre-code actions**, not the endpoint. Students should be able to name where they stayed in charge: they took the tour, reviewed and refined the plan, steered the session, approved the run, and merged, while the agent only ever *proposed*.
- Step 1 is the point where the course's deferred codebase exploration lands. Resist doing a manual code walkthrough beforehand; the exercise's design is that the agent gives the tour.
- **These deliverables are load-bearing for work that builds on this endpoint.** The `GET /customers/{id}` endpoint, `GetCustomerTests.cs`, and `GetCustomer.feature` with its steps are the style exemplars that the seeded backlog's task contracts cite by name. They must exist and be **merged** here, or the backlog those tasks describe will reference files that are not present. Do not let a session finish without all four deliverables, including the Swagger description update.
- Draw attention to step 6: agent pull requests do not auto-run workflows, so a student expecting instant green checks may think CI is stuck. Approving the run is the deliberate step that starts the `build-and-test` job.

# Lesson 2 — Task Contracts

## Scenario

Handing work to an agent by describing it in conversation does work, but the outcome rests on the agent reading our intent correctly. We say what we want, the agent fills in the gaps, and where its idea of the job differs from ours we tend to find out only when we read the diff. On a small, well-understood change that is tolerable. On a larger or vaguer one it is not.

What we want instead is a way of stating a task precisely enough that the result can be measured against something written down beforehand: what the agent is given to work from, what it must produce, and what would count as done. The same statement then serves twice, once as the instruction and once as the standard we judge the work by.

There is no shortage of work to try this on. The application still cannot update a customer, delete one, or list them all, and it knows nothing about products at all. GitHub gives us somewhere to keep that work, and a way to write each piece down properly before we hand it over.


## Concept — Agent Task Contracts

## Primer

When you hand work to an agent, specify it as a **contract** so the result can be checked against something concrete rather than hoped for. A task contract has three parts: **inputs**, the issue context, constraints, and boundaries the agent works within; **outputs**, what it must produce, namely a plan, a pull request, and evidence; and **success criteria**, how the result is judged, through checks, scans, and review outcomes. An under-specified task leaves the agent to guess at your intent, and a plausible but wrong change usually results. Write criteria that reflect what you actually want, such as "the described behaviour is correct", not merely "the tests pass". Passing CI is necessary but not sufficient.

You can also choose *when* to validate the work relative to the code. In **plan-first** delegation the agent opens a pull request containing only its plan, and you approve that plan before it writes any implementation, which suits high-risk work. In **plan-plus-execution** a single pull request carries the plan and commits together, which is faster and fits lower-risk work. Mixing the two invisibly leaves a reviewer with only a final diff and no chance to check intent early.

The pull request is the control point. Because every agent change arrives as one, its success criteria are what stand between a proposal and a merge.


## Exercise

# Exercise: Use a task contract

A contract only helps if every delegated task starts from one, so you inspect the repository's issue template, populate the backlog with contracted work, and then delegate a real task through it. The template already exists in the repository; you review its shape, seed the backlog from it, and delegate the first task on it to the cloud agent.

**Exercise:**

1. **Make sure you have pulled down the latest version of origin/main**. Also ensure it is synced with your session.
2. **Review the issue template form.** Open the application in VSCode. Navigate to `.github/ISSUE_TEMPLATE/agent-task.yml` and investigate its contents. Notice that it has the following:
   - **Context and inputs:** the background, the constraints, and the issue or code the task starts from.
   - **Expected outputs:** what the agent must produce, a plan, a pull request, and the evidence that the work is done.
   - **Success criteria:** how the result will be judged, the behaviour that must be correct and the checks that must pass.

3. In GitHub, navigate to Issues and click New Issue. See that there are two options: Agent Task and Blank issue. Agent Task is the issue template form.

4. **Seed the backlog.**  Run `scripts/seed-issues.ps1` to populate the repository issues with the backlog.

5. **Confirm the seeded issues.** Check that the backlog now holds five issues, and look over what each one asks for. Most carry the `agent-task` label and were filed through the contract shape. One is deliberately **off-template and unlabelled**, a loosely worded request with no `[Agent Task]:` prefix; leave it sitting in the backlog for now.

6. **Delegate the first contracted task.** Open **`[Agent Task]: Update customer`** and **assign it to the Copilot cloud agent**. Because it was filed as a contract, the agent receives the context, the expected outputs, the success criteria, and the boundaries without you re-describing them. Feel free to navigate to the agent's session to watch it run.

7. **Review the pull request against the contract, then merge.** When the session opens its pull request, read the diff clause by clause against the issue's success criteria: is the update behaviour correct, does `build-and-test` pass, are the boundaries respected? Note how much more decidable "done" is than it was for the conversationally specified retrieval task. Approve the run, confirm the checks, and **merge**.

By the end the repository holds a reusable **task-contract template**, a **seeded backlog** of contracted work, and the merged **update-customer endpoint** delivered through the first contract, so future agent work can start from an explicit specification rather than a loose description.

- The learning target is **specification**, not the update endpoint. Students should leave able to name a contract's four parts and say why an under-specified task tends to produce a plausible but wrong change. The clause-by-clause review in step 7 is the point: a contract makes the "is it done?" judgement concrete.
- This is the **first lesson to inspect the repository's issue template**, so introduce the contract shape here: the template is already authored in `.github/ISSUE_TEMPLATE/agent-task.yml`, and step 2 has students read it closely before they see it used. Reviewing an existing template, rather than authoring one from scratch, keeps the focus on recognising the four parts of a contract rather than on template-writing mechanics.
- **Sequencing is load-bearing:** `scripts/seed-issues.ps1` must run *after* the GET customer work is merged, because the seeded contract bodies cite `GetCustomer` files as style exemplars. If a student runs it earlier, those references dangle.
- Draw out that success criteria must reflect real intent. "Tests pass" alone is a weak contract; push students to name the behaviour that must be correct, and reinforce that CI green is necessary but not sufficient.
- The off-template `List customers` issue is left in place on purpose. It is the low-quality item the scheduled hygiene workflow is built to catch; do not tidy it away.

# Lesson 3 — Specialised Agents

## Scenario

The agent we delegate to is a generalist. It takes on whatever we give it, and it approaches a change to the API much as it approaches a piece of test writing, so how well each is done depends on how carefully we happen to word the request that day.

Deleting a customer shows the limit of that. It is really two jobs. One is backend work in .NET: the endpoint itself and the unit tests around it. The other is acceptance testing, written against the running API with its own conventions and its own idea of what good looks like. Spelled out in full, what each job needs to know is a lot, and on this approach we would be writing it out again every time.

We would rather that knowledge sat in the repository than in the prompt: one agent that knows how we build the API, another that knows how we write acceptance tests, each carrying its own role before we give it anything to do. GitHub lets us define those roles ourselves.


## Concept — Custom Agents

## Primer

A custom agent is a named, task-specific profile you define once and reuse: a system prompt, the tools it may call, a chosen model, and any MCP servers it can reach. The alternative is re-prompting one general-purpose agent for every job and restating the same conventions each time. A team can instead keep a roster of specialists, each carrying its own instructions.

You configure an agent in a `.agent.md` file. It carries a name and a description, which are what let someone choose it for a job. Tool access is set in the same file: you grant every tool, restrict it to a named list, or disable tools entirely, and that is how you bound what the agent is able to do. The model it runs on can be pinned there too.

When agents of the same name exist at more than one level, the closest one wins: a repository profile overrides an organisation profile, which overrides an enterprise profile. Once a task starts it pins to the version it began with for the life of the pull request, so the agent's behaviour does not shift mid-task.

Take a planning-only agent as an example. Its tools are limited to reading, searching, and editing, with no ability to run commands, so it can draft a plan but never execute code. Scoping the tools is what defines the role.


## Exercise

# Exercise: Build a specialised agent

The delete-customer work in your backlog splits into two disciplines, backend API development and acceptance testing, and a specialist is only worth building if it is both scoped for one job and reachable through the same contract-based delegation the repository's task-contract template defines. You author two named agent profiles, one per role, then dispatch each half of the delete work to the agent built for it. (A repository can also carry reusable skills that agents load on demand; this lesson stays with agent profiles and leaves skills aside.)

**Exercise:**

1. **Make sure you have pulled down main and synced it with your session**.
2. **Create a backend engineer agent.** Use the following prompt: Create a custom agent called .github/agents/backend-engineer.agent.md. It is a .NET specialist.

3. **Create an automation tester agent**. Use the following prompt: Create a custom agent called .github/agents/automation-tester.agent.md. It is a SpecFlow specialist.

4. **Inspect both profiles in VSCode.** Read the two `.agent.md` files and confirm the frontmatter is well formed, that neither carries a `model:` key, and that each system prompt confines the agent to its one discipline.

5. **Commit both, open a pull request, review, and merge** so the profiles live in the repository and can be assigned work.

6. **Dispatch the API half to `backend-engineer`.** Open **`[Agent Task]: Delete customer — API and unit tests`** and assign it to the **`backend-engineer`** agent. Its scope is the delete endpoint and its unit tests only; it must not touch the acceptance tests. When it opens its pull request, review the diff against the contract, confirm the checks, and **merge**.

7. **Dispatch the acceptance half to `automation-tester`, and leave its pull request open.** Open **`[Agent Task]: Delete customer — acceptance tests`** and assign it to the **`automation-tester`** agent. Its scope is the acceptance tests only; it must not touch the API source. Let it work and **raise its pull request, *then leave that pull request* *open***. You will return to it in the review work that follows; do not merge it here.

By the end the repository holds two reusable specialists, a **backend-engineer** agent and an **automation-tester** agent, each scoped to one discipline. The delete endpoint and its unit tests are merged, and the acceptance-testing pull request is **open and waiting**, the starting point for the review work to come.

- The learning target is **specialisation by configuration**: a custom agent is a scoped profile, and the `tools` restriction plus the system prompt are what make it a specialist rather than a generalist with a longer prompt. Students should leave able to say why a named, scoped profile beats re-prompting a generalist each time.
- **No `model:` key.** The repository's `agent.template.md` is the structure to follow; point out that omitting `model:` lets the platform default stand, which is the intended configuration here.
- Splitting the delete work across two agents is the whole point: `backend-engineer` owns the API and its unit tests, `automation-tester` owns the acceptance tests, and their boundaries are enforced by scope rather than by convention. Check that each agent's task assignment respects the other's territory.
- **Leave the acceptance-tests pull request open (step 5).** It is delivered as the `open-acceptance-pr` artefact and is consumed by the automated code-review work that reviews an open pull request; merging it here would remove the pull request that work opens on. This is deliberate, not an oversight to correct.
- Skills are acknowledged in one line only; do not expand into authoring a skill here. The focus is agent profiles.

# Lesson 4 — Configuring Code Review

## Scenario

A pull request is open on the repository, adding acceptance tests, and nobody has read it yet. Until someone does, the change is only proposed. Everything an agent produces arrives in this state, as a diff waiting for a person to say whether it is right.

One pull request is manageable. As the number grows it is less so, because reading every diff carefully takes longer than producing them, and we do not give every review the same care. The first pass is the part we would most like to hand over: a summary of what changed, and a note of the things most likely to be wrong.

There is also the question of our own conventions. A reviewer working from general good practice cannot know how this repository names things or how we expect code to be laid out, so it will pass over the very details a colleague would pick up. We would rather write those rules down once and have every review afterwards take account of them.


## Concept — GitHub Copilot Code Review

## Primer

Copilot code review is an **automated reviewer for pull requests**. It reads a pull request's diff and posts a change summary, inline comments on specific lines, and concerns flagged by severity, covering bugs, security issues, and style. It works alongside human reviewers rather than replacing them, offering a fast first pass over a change.

You get its review in one of two ways. You can **request it as a reviewer** on a particular pull request, or you can turn on an **auto-review setting** at the repository or organisation level so it runs on every pull request automatically. Its output uses the same interface as a person's review: many comments come with a suggested change you can apply in one click, and you can mark a comment helpful or unhelpful, reply to it (Copilot answers and can revise its suggestion), and resolve it.

The review is **diff-aware**: it judges the changed lines in the context of the surrounding file and, where relevant, the wider repository. Whether its approval counts towards the merge requirements is decided by branch protection, not by the review itself. To steer what it looks for, you can supply **custom coding guidelines**, a repository or organisation instructions file that points it at your team's conventions and the patterns you want flagged.


## Concept — Repository Custom Instructions

## Primer

Repository custom instructions let you give Copilot the conventions of a project once, in a file you commit, instead of restating them every time someone works in the repository. The main one is a single Markdown file at `.github/copilot-instructions.md`. Copilot on GitHub reads it and adds it to its requests automatically as soon as it is saved, so its behaviour follows your conventions without anyone pasting them into a prompt.

Because the file lives in the repository, it steers Copilot wherever Copilot acts on that repository, including the cloud agent and Copilot code review. Write a convention into it, such as a standard every pull request must observe, and Copilot applies that convention to every future request, with no per-request configuration.

Keep it short and general: guidance caps it at about two pages of repository-wide facts rather than task-specific detail.

Finer-grained forms exist alongside it: path-specific `NAME.instructions.md` files that apply only to matching paths, and `AGENTS.md` files scoped to their part of the tree. When several apply, their instructions combine rather than one replacing another.


## Exercise

# Exercise: Configure agentic code review

You pick up an open, agent-authored pull request and teach its reviewer one of your conventions before you ever run a review against it. You commit repository custom instructions to the branch, request a Copilot review that reads the rule before it judges the diff, then close the loop by handing what it flags back to the agent and merging.

**Exercise:**

1. **Open the standing acceptance pull request.** Return to the open, agent-authored acceptance-testing pull request. It adds SpecFlow acceptance tests to `CustomerManagement.AcceptanceTests` and has had no review yet. This is the change you will review.

2. **Open the branch in the Copilot app.** In the app, select 'Create from', then 'Pull requests' and choose the open PR. This will pull the branch down into a new session.

3. **Add repository custom instructions.** In that session, create `.github/copilot-instructions.md` at the repository root containing a single rule: **every private field must begin with `__`** (a double underscore). Push the changes to origin. See repository-custom-instructions.

4. **Request the review.** Request a Copilot review on the updated pull request. Because the branch now carries the instructions file, the review reads the double underscore rule before it judges the diff.

5. **Confirm the rule is observed.** Read the new review. Please be aware, AI is probabilistic. Sometimes, the observations about the coding standards can be found in 'Suppressed comments'.

6. **Hand the fix back to the cloud agent.** On one flagged finding, tag `@copilot` and ask it to bring the private fields into line with the rule. Merge when complete.

By the end the repository has a committed `.github/copilot-instructions.md` that steers code review with a project convention, and the agent-authored acceptance work has been reviewed, fixed, and merged.

- The learning target is **agentic code review shaped by your standards**: commit repository custom instructions before any review exists, request a single Copilot review that reads them, and watch it flag what violates the rule. Students should read the reviewer as a fast pass that focuses human attention, not a replacement for it.
- The `__` private-field rule is deliberately chosen. It is legal C# but **anti-idiomatic**, since the C# convention is `_camelCase`. Asking the reviewer to enforce a rule that contradicts the ecosystem default makes it visibly clear that the agent is obeying an explicit project convention rather than a habit. This is intentional; do not "correct" the rule to the idiomatic form.
- Adding the instructions is done in the **Copilot app, against the pull request's branch**, pulled down via 'Create from' > 'Pull requests' on the open PR (step 2). Pushing the instructions file to that branch is what puts it in front of the reviewer; if it lands anywhere else the requested review will not see it. Custom instructions can be hand-written or agent-drafted, per repository-custom-instructions.
- Do not block the room waiting on the `@copilot` fix in step 6. It is a small rename and can land during the lesson wrap-up or the break; students merge the pull request once it does. Closing the pull request out here is deliberate: a merge gate binds future work rather than legacy work, so this pull request is settled before any output-contract check exists.
- Branch protection and required-check gating are **out of scope** here; that is taught with agent-evaluation-signals. Keep this exercise to requesting the review and steering it with custom instructions.

# Lesson 5 — Evaluation Signals

## Scenario

When an agent finishes, what we get is a pull request: a diff, and a description written however the agent chose to write it. To judge it we read the description, then read the diff to see whether the two agree. Nothing about that is consistent between one pull request and the next.

A green build tells us that the tests which already existed still pass, which is not the same as showing that the change does what was asked, or that the agent checked its own work at all. We are still taking the agent's word for it, in prose.

We would rather every pull request came back in the same shape, saying what it set out to do and showing what proves it was done, and that anything falling short of that could not be merged. GitHub gives us both a place to ask for that and a way to enforce it.


## Concept — Agent Evaluation Signals

## Primer

Judging an agent's work starts by writing the success criteria into the issue or pull request before the work begins. Good criteria say what done actually means: the behaviour is correct, the tests pass, and no new security issues appear. They become the contract both the agent and the reviewer measure the result against, rather than a claim that the work is finished.

We judge that result from GitHub's own signals, not from the agent's confidence. The status checks, workflow runs, and Checks tab on a pull request show whether the change meets the standard, and each Actions run leaves its logs and results there for you to read. Treat a green pipeline as necessary but not sufficient: passing tests tells you the code runs, not that it does what you asked, so weigh the whole set of signals against the criteria you wrote.

Branch protection can make those signals mandatory instead of advisory. Its "require status checks to pass before merging" setting, with the specific checks selected, means a pull request cannot merge until they pass. Put security scanning in that required set as well, so a change cannot land while it introduces a flagged vulnerability. At that point the criteria you stated up front decide whether the work can merge at all.


## Exercise

# Exercise: Gate merges on evidence

You define what every agent pull request must return: a plan and evidence, in a predictable place, checked by a gate before the change can merge. You finish by running the new contract against another agent task. You work locally in the Copilot app.

**Exercise:**

1. **Ensure the latest version of main is pulled down and synced with your session.**

2. **Add a pull-request template.** Create `.github/PULL_REQUEST_TEMPLATE/pull_request_template.md` with a **Plan** section and an **Evidence** section, so every pull request states what it set out to do and shows the signals that confirm it is done. This is the output contract in agent-evaluation-signals made concrete on the pull request itself.

3. **Ask Copilot to build the gate.** Working locally in the Copilot app, ask it to create a GitHub Actions workflow `.github/workflows/plan-gate.yml` that runs on each pull request and verifies both the Plan and Evidence sections are present and carry **meaningful content**, failing the check when either is empty or left as a placeholder. Require a single job whose **job id is `plan-gate`**. Read the workflow it produces before you accept it.

4. **Mandate the template in your repository instructions.** Update `.github/copilot-instructions.md` to require that every pull request fills in the Plan and Evidence sections of the template, so agents author their pull requests to the contract from the start. See repository-custom-instructions.

5. **Commit the gate and merge it.** Commit the template, the workflow, and the updated instructions, open a pull request, review it, and merge. 

6. **Make the checks required.** Go to **Settings → Rules → Rulesets → New ruleset → New branch ruleset**. Name it `default-branch-gate` and set the enforcement status to **Active**. Under target branches choose **Include default branch**. Tick **Require status checks to pass**, then press '+ Add checks' and search for the plan-gate check and add it. 

7. **Run the contract against another agent task.** Open the seeded `[Agent Task]: Add product — API and unit tests` issue and assign it to the **backend-engineer** custom agent. Its work adds `POST /products` with unit tests.

8. **Judge the agent pull request on its plan and evidence.** When the agent raises its pull request, click **Approve and run workflows** in the merge box first. Actions workflows do not run automatically on a pull request the cloud agent opened, so until you approve them the required checks never report and the pull request sits at "Expected — waiting for status" with nothing in the merge box explaining why. Then read the Plan and Evidence sections it filled in, confirm the plan-gate check is green because they carry real content, and satisfy yourself the evidence matches what the task asked. Then **merge** it.

By the end the repository has an **output-contract gate**: a Plan/Evidence template, a plan-gate check, and a `default-branch-gate` ruleset that makes that check required, so a pull request carrying neither a plan nor evidence cannot merge. An agent-authored `POST /products` endpoint has come through the new contract and landed.

- The lesson **leads with** agent-evaluation-signals`#Primer`: a green build is necessary but not sufficient, and "done" is judged from platform evidence against explicit success criteria, not from an agent's confidence. The pull-request-template and plan-gate mechanics are taught **by doing them** in this exercise; the concept supplies the why, the exercise supplies the how.
- The learning target is the **output contract**: structuring what an agent returns and making the platform enforce it. Students should be able to point at the plan-gate check as the thing that turns "the pull request should explain itself" from a wish into a requirement.
- Steps 7 and 8 close the loop: the same contract that you defined now binds an agent pull request the students did not write, so the class sees the gate act on work it did not author by hand. If the agent's Plan or Evidence is thin, the gate should catch it, which is the point.
- Keep the plan-gate modest. A check that confirms both sections exist and are non-trivial is a complete success; engineering a sophisticated content analysis is out of scope.
- **The order of steps 5 and 6 is load-bearing and must not be moved earlier.** Creating the ruleset before the gate has merged deadlocks the very pull request that introduces it: a required status check that has never reported sits at "Expected — waiting for status" indefinitely, so the pull request can neither pass nor be merged. The picker compounds it, since it only offers check names the repository has been observed to report recently. Merging first supplies the name and clears both problems. `plan-gate.yml` is `on: pull_request`, so it does run on the pull request that adds it; at that point it reports without enforcing, which is exactly what is wanted.
- Leave the ruleset's bypass list empty, and check that students have not added themselves to it. Students are administrators of their own repository, and an administrator on the bypass list is offered an override in the merge box rather than a block, so a pull request that should be stopped merges anyway.
- Step 8's approval click is worth teaching rather than working around. Actions workflows are blocked from auto-running on cloud-agent pull requests until someone with write access approves them, which is propose-only autonomy showing its edges. Before the checks were required this was a mild inconvenience; now it is a hard block whose cause is invisible in the merge box, so students who do not know to look for the approval button will read it as the gate malfunctioning.
- The `gh` CLI cannot create a ruleset. `gh ruleset` exposes only `list`, `view`, and `check`, so creation goes through the REST API. The UI path in step 6 is the one to teach; the API shape below is for reference only, and its fiddly parts (the `~DEFAULT_BRANCH` ref-name token, and an optional `integration_id` per context to bind a check to the Actions app) teach nothing this lesson is about.
  ```bash
  gh api -X POST repos/{owner}/{repo}/rulesets \
    -f name='default-branch-gate' -f target='branch' -f enforcement='active' \
    -F 'conditions[ref_name][include][]=~DEFAULT_BRANCH' \
    -F 'rules[][type]=required_status_checks' \
    -F 'rules[][parameters][required_status_checks][][context]=plan-gate' \
    -F 'rules[][parameters][strict_required_status_checks_policy]=false'
  ```

# Lesson 6 — Agentic Workflows

## Scenario

The automation around a repository runs in response to events. Something is pushed, a pull request is opened, a check runs against it. Code fits that arrangement well, because code arrives through events, so there is always something to react to.

A great deal of what needs doing never fires an event. Issues arrive badly written and sit there, a build goes red and someone has to read the logs before anyone knows why, documentation drifts away from the code it describes, and nobody has a running picture of how the repository is doing. It is repetitive work that takes judgement, and none of it is reactive.

We would rather that work were handled continuously, across the repository and the things around it, than only when one of us opens a session and asks.

The open issues are where we start. A backlog is easy to check against a standard and worth checking often, and what works there works the same way for the documentation, the builds and the reporting. GitHub lets us keep that kind of automation in the repository as ordinary instructions, and have it run without anyone asking each time.


## Concept — Agentic Workflows

## Primer

An agentic workflow hands continuing repository work to an agent: triaging issues as they arrive, keeping documentation aligned with the code, improving test coverage, investigating failing builds and proposing fixes, reporting on repository health. It augments the build, test and release pipeline rather than replacing it, covering the subjective, repetitive judgement work that plain pipeline configuration struggles to express. It is another way of delegating to an agent, but one triggered and version-controlled like the rest of the pipeline rather than opened as a session and asked.

It is a markdown file kept in the repository. Its body is the plain-language instruction the agent follows, with no hidden template wrapped around it. Its frontmatter sets when it runs (`on`), what it may read (`permissions`), which writes it may make (`safe-outputs`), and which engine runs it (`engine`).

The markdown does not run directly. The `gh aw` command-line tool compiles it to a hardened `.lock.yml`, and that lock file is what Actions executes; both files are committed, so the lock file records exactly what the workflow can do. A workflow runs read-only unless a write is declared under `safe-outputs`, where each declared write names one permitted operation such as `create-issue`, `add-comment` or `add-labels`, and anything undeclared it cannot do. Any pull request it opens is never merged automatically.

You can write your own, or import one published by a trusted repository with `gh aw add`, which records its origin so you can pull upstream changes later.


## Exercise

# Exercise: Author an issue-hygiene workflow

You author a scheduled agent that reviews the repository's open issues on its own and flags the ones that do not meet your task-contract standard. The body of the markdown is the instruction the agent follows; the frontmatter fixes when it runs and the specific permissions it holds. You compile it, ship it through your gated path, and watch it catch the off-template issue that no event-driven guard ever saw. See github-agentic-workflows.

**Exercise:**

1. **Install and initialise the agentic-workflows tooling.** Add the extension and scaffold the repository:
   ```bash
   gh extension install github/gh-aw
   gh aw init
   ```
   `gh aw init` adds two dispatcher primitives to the repository: a skill at `.github/skills/agentic-workflows/SKILL.md` and a custom agent at `.github/agents/agentic-workflows.md`. These are the entry points the tooling uses to author and manage workflows. See github-agentic-workflows-cli-commands-reference.

2. **Author the issue-hygiene workflow.** Author an issue-hygiene workflow that runs daily at 9:00 am. On each run, it should review all open issues and apply a red NEEDS REVIEW label to any that do not follow the task-contract standard.

	Grant only these permissions:
	- contents: read
	- issues: read
	- pull-requests: read
	- copilot-requests: write

3. **Compile it.** Turn the markdown into the hardened workflow GitHub Actions will run:
   ```bash
   gh aw compile
   ```
   This produces a `.lock.yml` beside your `.md` in the same `.github/workflows/` directory. Open the lock file and review it.

4. **Ship it through your gated path.** Commit **both** the `.md` and its compiled `.lock.yml`, open a pull request, let your required checks run, then review the pull request and merge it the same as any other change. Both files must be committed for the workflow to run.

5. **Demonstrate it on the backlog.** Trigger the workflow manually rather than waiting for the next scheduled run. Watch it review the open issues and pick up the off-template `List customers` request, the casual unlabelled line in the backlog, and apply the red `NEEDS REVIEW` label to it. That labelled issue shows the workflow does what you wrote. See github-agentic-workflows-cli-commands-reference.

By the end the repository has an **issue-hygiene workflow**: a scheduled automation you wrote as a prompt, compiled to a reviewable lock file, shipped through your own gated path, and watched apply `NEEDS REVIEW` to the off-template issue that had sat unnoticed in the backlog.

- The learning target is **authoring a scheduled agentic workflow**: the markdown body is the prompt, the frontmatter fixes the cadence and the permissions, and the compile / commit-both / ship discipline puts a bespoke automation through the same gate as your code.
- The `List customers` issue is the target on purpose. It was seeded off-template and unlabelled, so no push-driven or pull-request-driven guard ever caught it; a scheduled workflow that reads the backlog is exactly the guard that does. This is the pay-off the scenario sets up.
- The permissions are **exactly** four: `contents: read` (to check out and read the repository), `issues: read` (to read the open-issue backlog the workflow reviews), `pull-requests: read` (part of the read surface the run needs for workflow context), and `copilot-requests: write` (the label is applied through the Copilot request, not a broad `issues: write` token). Together the block grants only the read/write surface the schedule needs, no broader write access — keep it exact and do not add permissions to "make labelling work".
- Commit **both** the `.md` and the `.lock.yml`. The lock file is generated by `gh aw compile` and must never be hand-edited; if the compiled schedule or permissions look wrong, the fix is always in the `.md`, followed by a re-compile.
- `gh aw init` scaffolds the dispatcher skill and agent named above; these are the tooling's own primitives, distinct from the workflow the student authors.
- Cost is out of scope: the billing model is unresolved in the wiki (github-agentic-workflows, Open questions), so do not teach cost estimation from this exercise.

# That's the course

Thank you for taking part. You now have a working repository, a set of delegated agent runs behind you, and a repeatable practice to keep building on.

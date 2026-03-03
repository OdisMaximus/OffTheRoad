# 🌌 CAVE2 Unity Project: Team Workflow

This repository is configured to **only** track the `Assets/ProjectAssets` folder. This keeps the repository lightweight by excluding the multi-gigabyte CAVE2 template library that we all already have installed locally.

---

## ⚠️ The Golden Rule
**ALL work must stay inside `Assets/ProjectAssets`.**

* **Scenes:** Save them in `ProjectAssets/Scenes`.
* **Scripts:** Save them in `ProjectAssets/Scripts`.
* **Prefabs:** Save them in `ProjectAssets/Prefabs`.

If you create or modify something outside of this folder, **Git will not see it**, and the rest of the team will not receive your updates. 

> **Note:** Do NOT modify the core CAVE2 template files. If you need to "practice" in those scenes, save a **copy** of the scene into the `ProjectAssets/Scenes` folder first.

---

## 🚀 First-Time Setup (Existing Project)
If you already have the CAVE2 template open on your machine, follow these steps to "overlay" our team's code onto your project:

1.  **Open Terminal:** Navigate to your **Main Unity Project Root** (the folder containing `Assets`, `Library`, etc.).
2.  **Initialize Git:**
    ```bash
    git init
    ```
3.  **Add Remote:**
    ```bash
    git remote add origin https://github.com/OdisMaximus/OffTheRoad.git
    ```
4.  **Fetch & Force Overlay:**
    ```bash
    git fetch
    git checkout -f main
    ```
    *(The `-f` is necessary because you already have local files that Git needs to ignore.)*

---

## 🔄 Daily Workflow
Before you start working:
```bash
git pull origin main
'''

# 🕹️ Idle Clicker Game – Unity Project

**Unity Version:** `2022.3.54f1`  
**Project Type:** `2D Core Gameplay`  

---

## 📌 Project Summary

This Unity project is a fully functional **Idle Clicker Game** developed with scalable architecture and responsive feedback systems. It uses a **Command Pattern** for clean action handling, integrates with a **MockAPI** for real-time leaderboard and player data management, and includes modern UI animations, vibrations, and upgrade logic.

---

## 🚀 Features

### 🧠 Architecture & Design

- ✅ **Command Pattern**: Clean decoupling of player input and system logic.
- ✅ **API Integration**:
  - `GET`: Fetch dynamic **leaderboard** and **player stats**.
  - `POST`: Push **new player entries** to the database (e.g., when a new player joins).
  - `PUT`: Update **existing player data** like balance and upgrade levels.
    
  ### 👤 Profile System (Local)

- 📱 **Local Profile Management** (No API involved):
  - Players can **create, assign, and switch profiles** on their device.
  - Profile data is stored locally using **PlayerPrefs** or **JSON-based file saving**.
  - Each profile stores: player name, upgrade levels, earned coins, and settings.
  - Enables **multi-user support on a single device** (useful for families or testing).
  - Fast switching without requiring internet connection or API calls.

### 🛒 Shop System

- In-game shop with:
  - Active (Tap-based) upgrades.
  - Passive (Idle-based) upgrades.
  - Dynamic cost scaling and level tracking.
  - Button-based event handling tied to upgrade actions.
  - Server sync after each purchase.

### 🎮 Gameplay & Feedback

- ⚡ **Tap to earn coins**, enhanced with multiplier visual effects.
- ⏳ **Idle income** generation over time.
- 🧮 **Level-based progression** with increasing rewards and costs.

### 🎨 Visuals & UX

- 🌀 **DoTween Animations**: Smooth transitions and animated UI feedback.
- 🎉 **Multiplier Effects**: Popping, scaling, and animated feedback on earning.
- 📳 **Vibration Feedback**: Added via **Vibration Master** plugin for physical button feedback.

---

## 🧰 Tech Stack

| Component             | Description                                  |
|-----------------------|----------------------------------------------|
| **Unity 2D**          | Base engine for game development             |
| **DoTween**           | Animation and tweening library               |
| **Vibration Master**  | Native vibration integration                 |
| **MockAPI**           | REST API used for data CRUD (GET, POST, PUT)|
| **C# Command Pattern**| Action system for encapsulated behaviors     |

---

## 🌐 API Usage

| Method | Purpose                          | Description                                        |
|--------|----------------------------------|----------------------------------------------------|
| `GET`  | Retrieve player data             | Gets current player levels and balance            |
| `POST` | Create new player data           | Pushes new player entries to the server           |
| `PUT`  | Update existing player progress  | Sends updated balance and upgrade levels          |

---




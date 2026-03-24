// app.js

// Constants
const ADMIN_PASSWORD = "123456";
const YEAR = "2026";
const MONTHS = [
  "January",
  "February",
  "March",
  "April",
  "May",
  "June",
  "July",
  "August",
  "September",
  "October",
  "November",
  "December",
];

// Bonus Values
const APPEALS_BONUS = {
  fullRefund: 4.0,
  partialRefund: 2.0,
  fee: 0.5,
  lost: 0.0,
};

const AMAZON_BONUS = {
  good: 50.0,
  minor: 15.0,
  bad: 0.0,
};

const CS_BONUS = {
  0: 50.0,
  1: 25.0,
  2: 0.0,
};

// State
let currentMonth = "2"; // Default to March (0-indexed, so 2)
let appData = {};

// Initialize App
function initApp() {
  loadData();
  populateMonthSelector();
  setupNavigation();
  setupEventListeners();
  updateAgentNamesUI();
  renderAll();
}

// Data Management
function loadData() {
  const data = localStorage.getItem("bonusTrackerData");
  if (data) {
    appData = JSON.parse(data);
  } else {
    // Starter Data for March 2026
    appData = {
      agents: {
        agent1: "Agent 1",
        agent2: "Agent 2",
      },
      records: {
        [YEAR]: {
          2: {
            // March
            appeals: [
              {
                id: Date.now() + 1,
                agent: "agent1",
                date: "2026-03-02",
                order: "111-1234567-8901234",
                platform: "Amazon",
                status: "completed",
                outcome: "fullRefund",
              },
              {
                id: Date.now() + 2,
                agent: "agent2",
                date: "2026-03-05",
                order: "1234567890",
                platform: "TikTok",
                status: "completed",
                outcome: "partialRefund",
              },
            ],
            amazon: { agent1: "good", agent2: "good" },
            cs: { agent1: "0", agent2: "0" },
            tiktok: [{ id: Date.now() + 3, date: "2026-03-01", score: 4.9 }],
          },
        },
      },
    };
    saveData();
  }

  // Ensure structure exists for current year
  if (!appData.records[YEAR]) appData.records[YEAR] = {};
}

function saveData() {
  localStorage.setItem("bonusTrackerData", JSON.stringify(appData));
}

function getMonthData(month) {
  if (!appData.records[YEAR][month]) {
    appData.records[YEAR][month] = {
      appeals: [],
      amazon: { agent1: "bad", agent2: "bad" }, // Default to $0
      cs: { agent1: "2", agent2: "2" }, // Default to $0
      tiktok: [],
    };
  }
  return appData.records[YEAR][month];
}

// UI Helpers
function populateMonthSelector() {
  const select = document.getElementById("global-month-select");
  select.innerHTML = "";
  MONTHS.forEach((name, index) => {
    const option = document.createElement("option");
    option.value = index;
    option.textContent = `${name} ${YEAR}`;
    if (index.toString() === currentMonth) option.selected = true;
    select.appendChild(option);
  });

  select.addEventListener("change", (e) => {
    currentMonth = e.target.value;
    renderAll();
  });
}

function setupNavigation() {
  const links = document.querySelectorAll(".nav-links li");
  const sections = document.querySelectorAll(".section-pane");

  links.forEach((link) => {
    link.addEventListener("click", () => {
      // Remove active classes
      links.forEach((l) => l.classList.remove("active"));
      sections.forEach((s) => s.classList.remove("active"));

      // Add active to clicked
      link.classList.add("active");
      document.getElementById(link.dataset.target).classList.add("active");
    });
  });
}

function updateAgentNamesUI() {
  document.querySelectorAll(".agent1-name").forEach((el) => {
    if (el.tagName === "INPUT") el.value = appData.agents.agent1;
    else el.textContent = appData.agents.agent1;
  });
  document.querySelectorAll(".agent2-name").forEach((el) => {
    if (el.tagName === "INPUT") el.value = appData.agents.agent2;
    else el.textContent = appData.agents.agent2;
  });
}

// Rendering
function renderAll() {
  renderSummary();
  renderAppeals();
  renderAmazon();
  renderCS();
  renderTikTok();
}

function getDaysInMonth(monthIndex) {
  return new Date(YEAR, parseInt(monthIndex) + 1, 0).getDate();
}

function calculateTikTokBonus() {
  const monthData = getMonthData(currentMonth);
  const daysInMonth = getDaysInMonth(currentMonth);
  let totalAssignedBonus = 0;

  monthData.tiktok.forEach((entry) => {
    let monthlyVal = 0;
    const s = parseFloat(entry.score);
    if (s <= 3.9) monthlyVal = 0;
    else if (s <= 4.0) monthlyVal = 20;
    else if (s <= 4.4) monthlyVal = 30;
    else if (s <= 4.6) monthlyVal = 60;
    else if (s <= 4.7) monthlyVal = 70;
    else if (s <= 4.8) monthlyVal = 80;
    else monthlyVal = 100;

    const duration = entry.duration || 1;
    totalAssignedBonus += (monthlyVal / daysInMonth) * duration;
  });

  return totalAssignedBonus;
}

function calculateTotals() {
  const monthData = getMonthData(currentMonth);

  // Appeals
  let appealsA1 = 0,
    appealsA2 = 0;
  monthData.appeals.forEach((a) => {
    if (a.status !== "inProgress") {
      const val = APPEALS_BONUS[a.outcome] || 0;
      if (a.agent === "agent1") appealsA1 += val;
      else appealsA2 += val;
    }
  });

  // Amazon
  const amzA1 = AMAZON_BONUS[monthData.amazon.agent1] || 0;
  const amzA2 = AMAZON_BONUS[monthData.amazon.agent2] || 0;

  // CS Quality
  const csA1 = CS_BONUS[monthData.cs.agent1] || 0;
  const csA2 = CS_BONUS[monthData.cs.agent2] || 0;

  // TikTok (Shared)
  const tiktokShared = calculateTikTokBonus();

  return {
    agent1: {
      appeals: appealsA1,
      amazon: amzA1,
      cs: csA1,
      tiktok: tiktokShared,
      total: appealsA1 + amzA1 + csA1 + tiktokShared,
    },
    agent2: {
      appeals: appealsA2,
      amazon: amzA2,
      cs: csA2,
      tiktok: tiktokShared,
      total: appealsA2 + amzA2 + csA2 + tiktokShared,
    },
  };
}

function renderSummary() {
  const totals = calculateTotals();

  // Cards
  const cardsContainer = document.getElementById("summary-cards-container");
  cardsContainer.innerHTML = `
        <div class="stat-card">
            <h3>${appData.agents.agent1} Total</h3>
            <div class="amount" style="color: var(--primary)">$${totals.agent1.total.toFixed(2)}</div>
        </div>
        <div class="stat-card">
            <h3>${appData.agents.agent2} Total</h3>
            <div class="amount" style="color: var(--success)">$${totals.agent2.total.toFixed(2)}</div>
        </div>
        <div class="stat-card">
            <h3>Combined Total</h3>
            <div class="amount">$${(totals.agent1.total + totals.agent2.total).toFixed(2)}</div>
        </div>
    `;

  // Breakdown Table
  const tbody = document.getElementById("summary-breakdown-body");
  tbody.innerHTML = `
        <tr>
            <td>Appeals</td>
            <td>$${totals.agent1.appeals.toFixed(2)}</td>
            <td>$${totals.agent2.appeals.toFixed(2)}</td>
        </tr>
        <tr>
            <td>Amazon Account Health</td>
            <td>$${totals.agent1.amazon.toFixed(2)}</td>
            <td>$${totals.agent2.amazon.toFixed(2)}</td>
        </tr>
        <tr>
            <td>Customer Service Quality</td>
            <td>$${totals.agent1.cs.toFixed(2)}</td>
            <td>$${totals.agent2.cs.toFixed(2)}</td>
        </tr>
        <tr>
            <td>TikTok Account Score (Shared)</td>
            <td>$${totals.agent1.tiktok.toFixed(2)}</td>
            <td>$${totals.agent2.tiktok.toFixed(2)}</td>
        </tr>
    `;
}

function renderAppeals() {
  const monthData = getMonthData(currentMonth);
  const tbody = document.getElementById("appeals-table-body");
  tbody.innerHTML = "";

  const filterAgent = document.getElementById("appeal-filter-agent")
    ? document.getElementById("appeal-filter-agent").value
    : "all";

  const outcomeLabels = {
    fullRefund: "Full Refund",
    partialRefund: "Partial Refund",
    fee: "Fee",
    lost: "Lost",
  };

  // Sort by date descending
  let sorted = [...monthData.appeals].sort(
    (a, b) => new Date(b.date) - new Date(a.date),
  );

  if (filterAgent !== "all") {
    sorted = sorted.filter((a) => a.agent === filterAgent);
  }

  sorted.forEach((a) => {
    const tr = document.createElement("tr");
    const statusLabel = a.status === "inProgress" ? "In Progress" : "Completed";
    const isCompleted = a.status !== "inProgress";
    const bonus = isCompleted ? APPEALS_BONUS[a.outcome] || 0 : 0;
    const outcomeDisp = isCompleted
      ? outcomeLabels[a.outcome] || a.outcome
      : "-";

    tr.innerHTML = `
            <td>${appData.agents[a.agent]}</td>
            <td>${a.date}</td>
            <td>${a.order}</td>
            <td>${a.platform}</td>
            <td><span class="badge ${a.status === "inProgress" ? "badge-warning" : "badge-success"}">${statusLabel}</span></td>
            <td>${outcomeDisp}</td>
            <td>$${bonus.toFixed(2)}</td>
            <td>
                <button class="btn btn-sm btn-secondary" onclick="editAppeal(${a.id})">Edit</button>
                <button class="btn btn-sm btn-danger" onclick="deleteAppeal(${a.id})">Delete</button>
            </td>
        `;
    tbody.appendChild(tr);
  });
}

function renderAmazon() {
  const monthData = getMonthData(currentMonth);
  document.getElementById("amazon-agent1").value =
    monthData.amazon.agent1 || "bad";
  document.getElementById("amazon-agent2").value =
    monthData.amazon.agent2 || "bad";
}

function renderCS() {
  const monthData = getMonthData(currentMonth);
  document.getElementById("cs-agent1").value = monthData.cs.agent1 || "2";
  document.getElementById("cs-agent2").value = monthData.cs.agent2 || "2";
}

function renderTikTok() {
  const monthData = getMonthData(currentMonth);
  const daysInMonth = getDaysInMonth(currentMonth);
  const tbody = document.getElementById("tiktok-table-body");
  tbody.innerHTML = "";

  let totalBonus = 0;

  // Sort by date desc
  const sorted = [...monthData.tiktok].sort(
    (a, b) => new Date(b.date) - new Date(a.date),
  );

  sorted.forEach((t) => {
    const s = parseFloat(t.score);
    let monthlyVal = 0;
    if (s <= 4.0) monthlyVal = 20;
    else if (s <= 4.4) monthlyVal = 30;
    else if (s <= 4.6) monthlyVal = 60;
    else if (s <= 4.7) monthlyVal = 70;
    else if (s <= 4.8) monthlyVal = 80;
    else monthlyVal = 100;

    const duration = t.duration || 1;
    const earned = (monthlyVal / daysInMonth) * duration;
    totalBonus += earned;

    const dateDisplay = duration === 7 ? `${t.date} (Week)` : t.date;

    const tr = document.createElement("tr");
    tr.innerHTML = `
            <td>${dateDisplay}</td>
            <td>${t.score}</td>
            <td>$${monthlyVal.toFixed(2)}</td>
            <td>+$${earned.toFixed(2)}</td>
            <td>
                <button class="btn btn-sm btn-danger" onclick="deleteTikTok(${t.id})">Delete</button>
            </td>
        `;
    tbody.appendChild(tr);
  });

  document.getElementById("tiktok-total-badge").textContent =
    `Total Bonus: $${totalBonus.toFixed(2)}`;
}

// Password Protection
let pendingAction = null;

function requestAdminAuth(callback) {
  pendingAction = callback;
  document.getElementById("password-modal").classList.add("active");
  document.getElementById("admin-password").value = "";
  document.getElementById("admin-password").focus();
  document.getElementById("password-error").style.display = "none";
}

function closePasswordModal() {
  document.getElementById("password-modal").classList.remove("active");
  pendingAction = null;
}

// Setup Event Listeners
function setupEventListeners() {
  // Password Form
  document.getElementById("password-form").addEventListener("submit", (e) => {
    e.preventDefault();
    const pwd = document.getElementById("admin-password").value;
    if (pwd === ADMIN_PASSWORD) {
      const actionToRun = pendingAction;
      closePasswordModal();
      if (actionToRun) actionToRun();
    } else {
      document.getElementById("password-error").style.display = "block";
    }
  });

  document
    .getElementById("btn-cancel-password")
    .addEventListener("click", closePasswordModal);

  // Add Appeal
  document.getElementById("appeal-status").addEventListener("change", (e) => {
    document.getElementById("appeal-outcome").disabled =
      e.target.value === "inProgress";
  });
  document.getElementById("appeal-outcome").disabled =
    document.getElementById("appeal-status").value === "inProgress";

  // Filter Agent
  const filterEl = document.getElementById("appeal-filter-agent");
  if (filterEl) {
    filterEl.addEventListener("change", renderAppeals);
  }

  document.getElementById("appeal-form").addEventListener("submit", (e) => {
    e.preventDefault();
    const monthData = getMonthData(currentMonth);
    monthData.appeals.push({
      id: Date.now(),
      agent: document.getElementById("appeal-agent").value,
      date: document.getElementById("appeal-date").value,
      order: document.getElementById("appeal-order").value,
      platform: document.getElementById("appeal-platform").value,
      status: document.getElementById("appeal-status").value,
      outcome: document.getElementById("appeal-outcome").value,
    });
    saveData();
    renderAll();
    e.target.reset();
    document.getElementById("appeal-outcome").disabled =
      document.getElementById("appeal-status").value === "inProgress";
  });

  // Save Amazon
  document.getElementById("amazon-form").addEventListener("submit", (e) => {
    e.preventDefault();
    const monthData = getMonthData(currentMonth);
    monthData.amazon.agent1 = document.getElementById("amazon-agent1").value;
    monthData.amazon.agent2 = document.getElementById("amazon-agent2").value;
    saveData();
    renderSummary(); // update totals

    const msg = document.getElementById("amazon-save-msg");
    msg.style.display = "inline";
    setTimeout(() => (msg.style.display = "none"), 2000);
  });

  // Save CS
  document.getElementById("cs-form").addEventListener("submit", (e) => {
    e.preventDefault();
    const monthData = getMonthData(currentMonth);
    monthData.cs.agent1 = document.getElementById("cs-agent1").value;
    monthData.cs.agent2 = document.getElementById("cs-agent2").value;
    saveData();
    renderSummary();

    const msg = document.getElementById("cs-save-msg");
    msg.style.display = "inline";
    setTimeout(() => (msg.style.display = "none"), 2000);
  });

  // Add TikTok Score
  document.getElementById("tiktok-form").addEventListener("submit", (e) => {
    e.preventDefault();
    const monthData = getMonthData(currentMonth);
    monthData.tiktok.push({
      id: Date.now(),
      date: document.getElementById("tiktok-date").value,
      score: document.getElementById("tiktok-score").value,
      duration: parseInt(document.getElementById("tiktok-type").value, 10),
    });
    saveData();
    renderAll();
    e.target.reset();
  });

  // Settings Forms
  document
    .getElementById("settings-agents-form")
    .addEventListener("submit", (e) => {
      e.preventDefault();
      appData.agents.agent1 = document.getElementById(
        "settings-agent1-name",
      ).value;
      appData.agents.agent2 = document.getElementById(
        "settings-agent2-name",
      ).value;
      saveData();
      updateAgentNamesUI();
      renderAll();

      const msg = document.getElementById("settings-save-msg");
      msg.style.display = "inline";
      setTimeout(() => (msg.style.display = "none"), 2000);
    });

  document.getElementById("btn-export").addEventListener("click", () => {
    const dataStr =
      "data:text/json;charset=utf-8," +
      encodeURIComponent(JSON.stringify(appData, null, 2));
    const dlAnchorElem = document.createElement("a");
    dlAnchorElem.setAttribute("href", dataStr);
    dlAnchorElem.setAttribute("download", "bonus_tracker_backup.json");
    dlAnchorElem.click();
  });

  document.getElementById("btn-import").addEventListener("change", (e) => {
    const file = e.target.files[0];
    if (!file) return;
    const reader = new FileReader();
    reader.onload = function (e) {
      try {
        const importedData = JSON.parse(e.target.result);
        if (importedData && importedData.agents && importedData.records) {
          appData = importedData;
          saveData();
          updateAgentNamesUI();
          renderAll();
          alert("Data imported successfully!");
        } else {
          alert("Invalid backup file format.");
        }
      } catch (err) {
        alert("Error reading file.");
      }
    };
    reader.readAsText(file);
  });

  document.getElementById("btn-clear-data").addEventListener("click", () => {
    requestAdminAuth(() => {
      if (
        confirm(
          "Are you sure you want to delete ALL data? This cannot be undone.",
        )
      ) {
        localStorage.removeItem("bonusTrackerData");
        location.reload();
      }
    });
  });
}

// Global actions for onclick
window.deleteAppeal = function (id) {
  requestAdminAuth(() => {
    const monthData = getMonthData(currentMonth);
    monthData.appeals = monthData.appeals.filter((a) => a.id !== id);
    saveData();
    renderAll();
  });
};

window.deleteTikTok = function (id) {
  requestAdminAuth(() => {
    const monthData = getMonthData(currentMonth);
    monthData.tiktok = monthData.tiktok.filter((t) => t.id !== id);
    saveData();
    renderAll();
  });
};

window.editAppeal = function (id) {
  const monthData = getMonthData(currentMonth);
  const appeal = monthData.appeals.find((a) => a.id === id);
  if (!appeal) return;

  // Populate modal form
  document.getElementById("edit-appeal-id").value = appeal.id;
  document.getElementById("edit-appeal-agent").value = appeal.agent;
  document.getElementById("edit-appeal-date").value = appeal.date;
  document.getElementById("edit-appeal-order").value = appeal.order;
  document.getElementById("edit-appeal-platform").value = appeal.platform;

  const statusVal = appeal.status || "completed";
  document.getElementById("edit-appeal-status").value = statusVal;
  document.getElementById("edit-appeal-outcome").value =
    appeal.outcome || "fullRefund";
  document.getElementById("edit-appeal-outcome").disabled =
    statusVal === "inProgress";

  // Show modal
  document.getElementById("edit-appeal-modal").classList.add("active");
};

document
  .getElementById("edit-appeal-status")
  .addEventListener("change", (e) => {
    document.getElementById("edit-appeal-outcome").disabled =
      e.target.value === "inProgress";
  });

document
  .getElementById("btn-cancel-edit-appeal")
  .addEventListener("click", () => {
    document.getElementById("edit-appeal-modal").classList.remove("active");
  });

document.getElementById("edit-appeal-form").addEventListener("submit", (e) => {
  e.preventDefault();
  const id = parseInt(document.getElementById("edit-appeal-id").value);
  const monthData = getMonthData(currentMonth);
  const index = monthData.appeals.findIndex((a) => a.id === id);
  if (index !== -1) {
    monthData.appeals[index] = {
      id: id,
      agent: document.getElementById("edit-appeal-agent").value,
      date: document.getElementById("edit-appeal-date").value,
      order: document.getElementById("edit-appeal-order").value,
      platform: document.getElementById("edit-appeal-platform").value,
      status: document.getElementById("edit-appeal-status").value,
      outcome: document.getElementById("edit-appeal-outcome").value,
    };
    saveData();
    renderAll();
  }
  document.getElementById("edit-appeal-modal").classList.remove("active");
});

// Start app
document.addEventListener("DOMContentLoaded", initApp);

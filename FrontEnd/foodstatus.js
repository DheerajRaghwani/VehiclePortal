const apiUrl = "https://localhost:7291/api/Foodstatus"; // Update with your API URL
const token = localStorage.getItem("token"); // Assuming token is stored

// DOM Elements
const sourcePassEntryBtn = document.getElementById("sourcePassEntryBtn");
const viewEntryBtn = document.getElementById("viewEntryBtn");
const pendingListBtn = document.getElementById("pendingListBtn");
const logoutBtn = document.getElementById("logoutBtn");
const tableSection = document.getElementById("tableSection");
const foodstatusTableBody = document.querySelector("#foodstatusTable tbody");
const vehicleNoInput = document.getElementById("vehicleNoInput");
const districtDropdown = document.getElementById("districtDropdown");
const blockDropdown = document.getElementById("blockDropdown");
const searchBtn = document.getElementById("searchBtn");
const resetBtn = document.getElementById("resetBtn");
const editModal = document.getElementById("editModal");
const closeModal = document.querySelector(".close");
const editForm = document.getElementById("editForm");
const passInput = document.getElementById("passInput");
const totalPeopleInput = document.getElementById("totalPeopleInput");

let currentEditId = null;

// Show table section
sourcePassEntryBtn.addEventListener("click", () => {
  tableSection.classList.remove("hidden");
  loadDistricts();
  loadTable(); // Load all initially
});

// Logout
logoutBtn.addEventListener("click", () => {
  localStorage.removeItem("token");
  window.location.href = "login.html"; // redirect to login
});

// Close modal
closeModal.addEventListener("click", () => {
  editModal.classList.add("hidden");
});

// Load Districts
async function loadDistricts() {
  districtDropdown.innerHTML = '<option value="">Select District</option>';
  try {
    const res = await fetch(`${apiUrl}/DistrictDropdown`, { headers: { Authorization: `Bearer ${token}` } });
    const districts = await res.json();
    districts.forEach(d => {
      const option = document.createElement("option");
      option.value = d.districtName;
      option.textContent = d.districtName;
      districtDropdown.appendChild(option);
    });
  } catch (err) {
    console.error(err);
  }
}

// Load Blocks based on District
districtDropdown.addEventListener("change", async () => {
  const districtName = districtDropdown.value;
  blockDropdown.innerHTML = '<option value="">Select Block</option>';
  if (!districtName) return;
  try {
    const res = await fetch(`${apiUrl}/District/${districtName}`, { headers: { Authorization: `Bearer ${token}` } });
    const blocks = await res.json();
    blocks.forEach(b => {
      const option = document.createElement("option");
      option.value = b.blockname;
      option.textContent = b.blockname;
      blockDropdown.appendChild(option);
    });
  } catch (err) {
    console.error(err);
  }
});

// Load Table
async function loadTable(filters = {}) {
  foodstatusTableBody.innerHTML = "";
  let url = `${apiUrl}/All`;

  // Optional search filters
  if (filters.vehicleNo) url += `?vehicleNo=${filters.vehicleNo}`;

  try {
    const res = await fetch(url, { headers: { Authorization: `Bearer ${token}` } });
    let data = await res.json();

    // Apply district/block filters
    if (filters.districtName) data = data.filter(d => d.districtName === filters.districtName);
    if (filters.blockName) data = data.filter(d => d.blockName === filters.blockName);

    data.forEach(item => {
      const tr = document.createElement("tr");
      tr.innerHTML = `
        <td>${item.vehicleNo}</td>
        <td>${item.vehicleType || ""}</td>
        <td>${item.driverName || ""}</td>
        <td>${item.seatCapacity || ""}</td>
        <td>${item.districtName || ""}</td>
        <td>${item.blockName || ""}</td>
        <td>${item.pass === true ? "Yes" : item.pass === false ? "No" : ""}</td>
        <td>${item.totalPeople || ""}</td>
        <td>
          <button class="action-btn edit-btn" data-id="${item.id}">Edit</button>
          <button class="action-btn delete-btn" data-id="${item.vehicleNo}">Delete</button>
        </td>
      `;
      foodstatusTableBody.appendChild(tr);
    });

    // Attach Edit/Delete Events
    document.querySelectorAll(".edit-btn").forEach(btn => {
      btn.addEventListener("click", () => openEditModal(btn.dataset.id));
    });
    document.querySelectorAll(".delete-btn").forEach(btn => {
      btn.addEventListener("click", () => deleteEntry(btn.dataset.id));
    });
  } catch (err) {
    console.error(err);
  }
}

// Search Button
searchBtn.addEventListener("click", () => {
  loadTable({
    vehicleNo: vehicleNoInput.value.trim(),
    districtName: districtDropdown.value,
    blockName: blockDropdown.value
  });
});

// Reset Button
resetBtn.addEventListener("click", () => {
  vehicleNoInput.value = "";
  districtDropdown.value = "";
  blockDropdown.value = "";
  loadTable();
});

// Edit Modal
async function openEditModal(id) {
  currentEditId = id;
  // Fetch current data
  const res = await fetch(`${apiUrl}/GetById/${id}`, { headers: { Authorization: `Bearer ${token}` } });
  const data = await res.json();
  passInput.value = data.pass;
  totalPeopleInput.value = data.totalPeople || 0;
  editModal.classList.remove("hidden");
}

// Submit Edit Form
editForm.addEventListener("submit", async (e) => {
  e.preventDefault();
  const payload = {
    pass: passInput.value === "true",
    totalPeople: parseInt(totalPeopleInput.value)
  };
  try {
    const res = await fetch(`${apiUrl}/Update/${currentEditId}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        "Authorization": `Bearer ${token}`
      },
      body: JSON.stringify(payload)
    });
    if (!res.ok) throw new Error("Update failed");
    editModal.classList.add("hidden");
    loadTable();
  } catch (err) {
    console.error(err);
  }
});

// Delete Entry
async function deleteEntry(vehicleNo) {
  if (!confirm("Are you sure to delete this entry?")) return;
  try {
    const res = await fetch(`${apiUrl}/${vehicleNo}`, {
      method: "DELETE",
      headers: { Authorization: `Bearer ${token}` }
    });
    if (!res.ok) throw new Error("Delete failed");
    loadTable();
  } catch (err) {
    console.error(err);
  }
}

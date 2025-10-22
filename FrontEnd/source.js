const apiBaseUrl = "https://localhost:7005/api/Source";
const districtApi = "https://localhost:7005/api/District";
const blockApi = "https://localhost:7005/api/Block";

// Elements
const vehicleNoInput = document.getElementById("vehicleNoInput");
const resetBtn = document.getElementById("resetBtn");
const logoutBtn = document.getElementById("logoutBtn");
const vehicleTableBody = document.querySelector("#vehicleTable tbody");
const districtSelect = document.getElementById("districtSelect");
const blockSelect = document.getElementById("blockSelect");

const sourceModal = document.getElementById("sourceModal");
const modalVehicleNo = document.getElementById("modalVehicleNo");
const totalPeople = document.getElementById("totalPeople");
const passOption = document.getElementById("passOption");
const addSourceBtn = document.getElementById("addSourceBtn");
const cancelSourceBtn = document.getElementById("cancelSourceBtn");

const sourceVehicleBtn = document.getElementById("sourceVehicleBtn");
const sourceListModal = document.getElementById("sourceListModal");
const closeSourceListModal = document.getElementById("closeSourceListModal");
const sourceListTableBody = document.querySelector("#sourceListTable tbody");

const cancelSourceListBtn = document.getElementById("cancelSourceListBtn");

let lastSearchedVehicle = null;

/* ------------------ Counts ------------------ */
let totalVehicleCount = 0;
let passedVehicleCount = 0;

function updateCounts() {
    const totalEl = document.getElementById("totalVehicles");
    const passedEl = document.getElementById("passedVehicles");
    
    if (!totalEl || !passedEl ) return; // only update if elements exist
	 const total = Math.max(0, totalVehicleCount);
    const passed = Math.max(0, passedVehicleCount);
     

    totalEl.textContent = total;
    passedEl.textContent = passed;
    
}

/* ------------------ Load Districts ------------------ */
async function loadDistricts() {
    try {
        const res = await fetch(`${districtApi}/GetAll`, {
            headers: { "Authorization": `Bearer ${localStorage.getItem("token")}` }
        });
        const data = await res.json();
        districtSelect.innerHTML = '<option value="">Select District</option>';
        (data.data || data).forEach(d => {
            const option = document.createElement("option");
            option.value = d.districtName || d.DistrictName;
            option.textContent = d.districtName || d.DistrictName;
            districtSelect.appendChild(option);
        });
    } catch (err) { console.error(err); }
}

districtSelect.addEventListener("change", searchVehicle);
blockSelect.addEventListener("change", searchVehicle);
vehicleNoInput.addEventListener("input", searchVehicle);

/* ------------------ Load Blocks by District ------------------ */
districtSelect.addEventListener("change", async () => {
    const districtName = districtSelect.options[districtSelect.selectedIndex].text;
    blockSelect.innerHTML = '<option value="">Select Block</option>';

    if (!districtName) return;

    try {
        const res = await fetch(`${blockApi}/by-district-name/${encodeURIComponent(districtName)}`, {
            headers: { "Authorization": `Bearer ${localStorage.getItem("token")}` }
        });

        if (!res.ok) throw new Error(`API Error: ${res.status}`);
        const blocks = await res.json();

        blocks.forEach(b => {
            const option = document.createElement("option");
            option.value = b.blockname;
            option.textContent = b.blockname;
            blockSelect.appendChild(option);
        });

        searchVehicle();
    } catch (err) {
        console.error(err);
        alert("Error loading blocks");
    }
});

/* ------------------ Load All Vehicles ------------------ */
async function loadAllVehicles() {
    try {
        const res = await fetch(`${apiBaseUrl}/vehicles/all`, {
            headers: { "Authorization": `Bearer ${localStorage.getItem("token")}` }
        });
        const data = await res.json();
        displayVehicles(data);
    } catch (err) { console.error(err); }
}

/* ------------------ Display Vehicles ------------------ */
function displayVehicles(data) {
    vehicleTableBody.innerHTML = "";
    if (!data || data.length === 0) {
        vehicleTableBody.innerHTML = `<tr><td colspan="6">No vehicles found</td></tr>`;
        totalVehicleCount = 0;
        updateCounts();
        return;
    }

    totalVehicleCount = data.length;
    updateCounts();

    data.forEach(v => {
        const row = document.createElement("tr");
		
        row.innerHTML = `
            <td>${v.vehicleNo}</td>
            <td>${v.districtName || "-"}</td>
            <td>${v.blockName || "-"}</td>
            <td>${v.vehicleType || "-"}</td>
            <td>${v.seatCapacity || "-"}</td>
			<td>${v.vehicleNodalName || "-"}</td>
            <td>${v.nodalMobileNo || "-"}</td>
            <td><button class="sourceBtn" data-vehicleno="${v.vehicleNo}">Village Departure</button></td>
        `;
        vehicleTableBody.appendChild(row);
    });

    document.querySelectorAll(".sourceBtn").forEach(btn => {
        btn.addEventListener("click", openSourceModal);
    });
}

/* ------------------ Filter Vehicles ------------------ */
async function searchVehicle() {
    const vehicleNo = vehicleNoInput.value.trim().toLowerCase();
    const district = districtSelect.value;
    const block = blockSelect.value;

    const res = await fetch(`${apiBaseUrl}/vehicles/all`, {
        headers: { "Authorization": `Bearer ${localStorage.getItem("token")}` }
    });
    const data = await res.json();

    const filtered = data.filter(v => {
        const matchVehicle = vehicleNo ? v.vehicleNo.toLowerCase().includes(vehicleNo) : true;
        const matchDistrict = district ? v.districtName === district : true;
        const matchBlock = block ? v.blockName === block : true;
        return matchVehicle && matchDistrict && matchBlock;
    });

    displayVehicles(filtered);
}

/* ------------------ Reset ------------------ */
resetBtn.addEventListener("click", () => {
    vehicleNoInput.value = "";
    districtSelect.value = "";
    blockSelect.value = "";
    vehicleTableBody.innerHTML = "";
    loadAllVehicles();
});

/* ------------------ Logout ------------------ */
logoutBtn.addEventListener("click", () => {
    localStorage.removeItem("token");
    window.location.href = "login.html";
});

/* ------------------ Source Modal ------------------ */
function openSourceModal(e) {
    const vehicleNo = e.target.dataset.vehicleno;
    modalVehicleNo.textContent = vehicleNo;
    totalPeople.value = 1;
    passOption.value = "";
    addSourceBtn.dataset.vehicleNo = vehicleNo;
    sourceModal.style.display = "flex";
}

cancelSourceBtn.addEventListener("click", () => {
    sourceModal.style.display = "none";
});

/* ------------------ Submit Source ------------------ */
addSourceBtn.addEventListener("click", async () => {
    const vehicleNo = addSourceBtn.dataset.vehicleNo;
    const pass = true;
    const people = parseInt(totalPeople.value) || 0;

    if (!vehicleNo) return alert("Vehicle number missing");
   
    if (!people) return alert("Enter valid total people");

    const payload = { VehicleNo: vehicleNo, Pass: pass, TotalPeople: people };

    try {
        const res = await fetch(`${apiBaseUrl}/Add`, {
            method: "POST",
            headers: { "Authorization": `Bearer ${localStorage.getItem("token")}`, "Content-Type": "application/json" },
            body: JSON.stringify(payload)
        });
        const text = await res.text();
        let result = {};
        try { result = JSON.parse(text); } catch { result = { message: text }; }

        if (!res.ok) {
            if (text.toLowerCase().includes("duplicate") || text.toLowerCase().includes("unique")) {
                alert("⚠️ Duplicate entry: This vehicle already has a pass record.");
            } else {
                alert(result.message || "Error adding source record.");
            }
            return;
        }

        alert(result.message || "Source record added!");
        sourceModal.style.display = "none";
        searchVehicle();
        await loadSourceRecords(); // refresh passed count
    } catch (err) { console.error(err); alert("Error adding source record"); }
});

/* ------------------ Source Records Modal ------------------ */
sourceVehicleBtn.addEventListener("click", async () => {
    sourceListModal.style.display = "flex";
    await loadSourceRecords();
});

closeSourceListModal.addEventListener("click", () => {
    sourceListModal.style.display = "none";
});

cancelSourceListBtn.addEventListener("click", () => {
    sourceListModal.style.display = "none";
});

/* ------------------ Load Source Records ------------------ */
async function loadSourceRecords() {
    const res = await fetch(`${apiBaseUrl}/GetAll`, {
        headers: { "Authorization": `Bearer ${localStorage.getItem("token")}` }
    });
    const data = await res.json();
    displaySourceRecords(data);
}

/* ------------------ Display Source Records ------------------ */
function displaySourceRecords(data) {
    sourceListTableBody.innerHTML = "";
    if (!data || data.length === 0) {
        sourceListTableBody.innerHTML = `<tr><td colspan="6" style="text-align:center;">No records found</td></tr>`;
        passedVehicleCount = 0;
        updateCounts();
        return;
    }

    passedVehicleCount = data.length;
    updateCounts();

    data.forEach(r => {
        const row = document.createElement("tr");

        // Format date + time
        let formattedDateTime = "-";
        if (r.currentDate) {
            const date = new Date(r.currentDate);
            const options = {
                year: "numeric",
                month: "short",
                day: "2-digit",
                hour: "2-digit",
                minute: "2-digit",
                second: "2-digit",
                hour12: true
            };
            formattedDateTime = date.toLocaleString(undefined, options);
        }

        row.innerHTML = `
            <td>${r.vehicleNo || "-"}</td>
            <td>${r.pass ? "Yes" : "No"}</td>
            <td>${r.totalPeople || "-"}</td>
            <td>${r.vehicleNodalName || "-"}</td>
            <td>${r.nodalMobileNo || "-"}</td>
            <td>${formattedDateTime}</td>
        `;
        sourceListTableBody.appendChild(row);
    });
}
// ------------------ Search Passed Vehicles ------------------
const searchPassedVehiclesInput = document.getElementById("searchPassedVehicles");
const passedListTableBody = document.querySelector("#sourceListTable tbody");

let passedSearchTimeout;
searchPassedVehiclesInput.addEventListener("input", () => {
    clearTimeout(passedSearchTimeout);
    passedSearchTimeout = setTimeout(searchPassedVehicles, 300);
});

async function searchPassedVehicles() {
    const vehicleNo = searchPassedVehiclesInput.value.trim();

    if (!vehicleNo) {
        loadAllPassedVehicles();
        return;
    }

    try {
        const res = await fetch(`${apiBaseUrl}/search?vehicleNo=${encodeURIComponent(vehicleNo)}`, {
            headers: { "Authorization": `Bearer ${localStorage.getItem("token")}` }
        });

        if (!res.ok) throw new Error(`API Error: ${res.status}`);
        const data = await res.json();
        displayPassedVehicles(data);
    } catch (err) {
        console.error("Error searching passed vehicles:", err);
        passedListTableBody.innerHTML = `<tr><td colspan="6" style="text-align:center;">Error fetching vehicles</td></tr>`;
    }
}

async function loadAllPassedVehicles() {
    try {
        const res = await fetch(`${apiBaseUrl}/GetAll`, {
            headers: { "Authorization": `Bearer ${localStorage.getItem("token")}` }
        });
        if (!res.ok) throw new Error(`API Error: ${res.status}`);
        const data = await res.json();
        displayPassedVehicles(data);
    } catch (err) {
        console.error("Error loading passed vehicles:", err);
        passedListTableBody.innerHTML = `<tr><td colspan="6" style="text-align:center;">Error loading vehicles</td></tr>`;
    }
}

function displayPassedVehicles(data) {
    passedListTableBody.innerHTML = "";

    if (!data || data.length === 0) {
        passedListTableBody.innerHTML = `<tr><td colspan="6" style="text-align:center;">No records found</td></tr>`;
        return;
    }

    data.forEach(r => {
        const row = document.createElement("tr");
        row.innerHTML = `
            <td>${r.vehicleNo || "-"}</td>
            <td>${r.pass || "-"}</td>
            <td>${r.totalPeople || "-"}</td>
            <td>${r.vehicleNodalName || "-"}</td>
            <td>${r.nodalMobileNo || "-"}</td>
            <td>${r.currentDate ? new Date(r.currentDate).toLocaleDateString() : "-"}</td>
        `;
        passedListTableBody.appendChild(row);
    });
}



/* ------------------ Initial Load ------------------ */
loadDistricts();
loadAllVehicles();
loadSourceRecords();

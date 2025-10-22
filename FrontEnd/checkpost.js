/* ------------------ API URLs ------------------ */
const apiBaseUrl = "https://localhost:7005/api/Checkpost";
const districtApi = "https://localhost:7005/api/District";
const blockApi = "https://localhost:7005/api/Block";
const checkApi = "https://localhost:7005/api/Checkpostname";

/* ------------------ DOM Elements ------------------ */
const vehicleNoInput = document.getElementById("vehicleNoInput");
const resetBtn = document.getElementById("resetBtn");
const logoutBtn = document.getElementById("logoutBtn");
const vehicleTableBody = document.querySelector("#vehicleTable tbody");
const allEntries = document.getElementById("allEntries");
const districtSelect = document.getElementById("districtSelect");
const blockSelect = document.getElementById("blockSelect");

const passModal = document.getElementById("passModal");
const passedListModal = document.getElementById("passedListModal");

const modalVehicleNo = document.getElementById("modalVehicleNo");
const passDate = document.getElementById("passDate");
const checkpostSelect = document.getElementById("checkpostSelect");
const addPassBtn = document.getElementById("addPassBtn");
const passOption = document.getElementById("passOption");

const cancelPassBtn = document.getElementById("cancelPassBtn");
const closePassModalBtn = document.getElementById("closePassModal");

const passedVehicleBtn = document.getElementById("passedVehicleBtn");
const closePassedListModalBtn = document.getElementById("closePassedListModal");
const passedListTableBody = document.querySelector("#passedListTable tbody");

const addCheckpostBtn = document.getElementById("addCheckpostBtn");
const newCheckpostDiv = document.getElementById("newCheckpostDiv");
const newCheckpostInput = document.getElementById("newCheckpostInput");
const saveCheckpostBtn = document.getElementById("saveCheckpostBtn");

/* ------------------ Counts ------------------ */
let totalVehicleCount = 0;
let passedVehicleCount = 0;

function updateCounts() {
    const totalEl = document.getElementById("totalVehicles");
   
    const passedElModal = document.getElementById("passedVehiclesModal"); // modal
 

    if (!totalEl || !passedElModal ) return;

    const total = Math.max(0, totalVehicleCount);
    const passed = Math.max(0, passedVehicleCount);
    

    totalEl.textContent = total;
    passedElModal.textContent = passed;
}

/* ------------------ Load Districts ------------------ */
async function loadDistricts() {
    try {
        const res = await fetch(`${districtApi}/GetAll`, {
            headers: { "Authorization": `Bearer ${localStorage.getItem("token")}` }
        });
        if (!res.ok) throw new Error(`API Error: ${res.status}`);
        const data = await res.json();
        const districts = Array.isArray(data) ? data : data.data;

        districtSelect.innerHTML = '<option value="">Select District</option>';
        districts.forEach(d => {
            const option = document.createElement("option");
            option.value = d.districtName || d.DistrictName;
            option.textContent = d.districtName || d.DistrictName;
            districtSelect.appendChild(option);
        });
    } catch (err) {
        console.error("Error loading districts:", err);
        alert("Error loading districts. Check console.");
    }
}

/* ------------------ Load Blocks by District ------------------ */
districtSelect.addEventListener("change", async () => {
    const districtName = districtSelect.value;
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

/* ------------------ Filter Vehicles ------------------ */
vehicleNoInput.addEventListener("input", searchVehicle);
blockSelect.addEventListener("change", searchVehicle);

/* ------------------ Load All Vehicles ------------------ */
async function loadAllVehicles() {
    try {
        const res = await fetch(`${apiBaseUrl}/vehicles/all`, {
            headers: { "Authorization": `Bearer ${localStorage.getItem("token")}` }
        });
        if (!res.ok) throw new Error(`API Error: ${res.status}`);
        const data = await res.json();
        displayVehicles(data);
    } catch (err) {
        console.error("Error loading vehicles:", err);
        vehicleTableBody.innerHTML = `<tr><td colspan="6" style="text-align:center;">Error loading vehicles</td></tr>`;
    }
}

/* ------------------ Display Vehicles ------------------ */
function displayVehicles(data) {
    vehicleTableBody.innerHTML = "";

    if (!Array.isArray(data) || data.length === 0) {
        vehicleTableBody.innerHTML = `<tr><td colspan="6" style="text-align:center;">No vehicles found</td></tr>`;
        totalVehicleCount = 0;
        updateCounts();
        return;
    }

    totalVehicleCount = data.length;
    updateCounts();

    data.forEach(v => {
        const row = document.createElement("tr");

        row.innerHTML = `
            <td>${v.vehicleNo || "-"}</td>
            <td>${v.districtName || "-"}</td>
            <td>${v.blockName || "-"}</td>
            <td>${v.vehicleType || "-"}</td>
            <td>${v.vehicleCapacity || "-"}</td>
			<td>${v.vehicleNodalName || "-"}</td>
			<td>${v.nodalMobileNo || "-"}</td>
            <td><button class="passBtn" data-vehicleno="${v.vehicleNo}">CheckPost Crossing</button></td>
        `;
        vehicleTableBody.appendChild(row);
    });

    document.querySelectorAll(".passBtn").forEach(btn => {
        btn.addEventListener("click", openPassModal);
    });
}

/* ------------------ Search Vehicles ------------------ */
async function searchVehicle() {
    const vehicleNo = vehicleNoInput.value.trim().toLowerCase();
    const district = districtSelect.value;
    const block = blockSelect.value;

    try {
        const res = await fetch(`${apiBaseUrl}/vehicles/all`, {
            headers: { "Authorization": `Bearer ${localStorage.getItem("token")}` }
        });
        if (!res.ok) throw new Error(`API Error: ${res.status}`);
        const data = await res.json();

        const filtered = data.filter(v => {
            const matchVehicle = vehicleNo ? v.vehicleNo.toLowerCase().includes(vehicleNo) : true;
            const matchDistrict = district ? v.districtName === district : true;
            const matchBlock = block ? v.blockName === block : true;
            return matchVehicle && matchDistrict && matchBlock;
        });

        displayVehicles(filtered);
    } catch (err) {
        console.error(err);
        vehicleTableBody.innerHTML = `<tr><td colspan="6" style="text-align:center;">Error loading vehicles</td></tr>`;
    }
}

/* ------------------ Reset & Logout ------------------ */
resetBtn.addEventListener("click", () => {
    vehicleNoInput.value = "";
    districtSelect.value = "";
    blockSelect.innerHTML = '<option value="">Select Block</option>';
    vehicleTableBody.innerHTML = "";
    allEntries.innerHTML = "";
    loadAllVehicles();
});

logoutBtn.addEventListener("click", () => {
    localStorage.removeItem("token");
    window.location.href = "login.html";
});

/* ------------------ Open Pass Modal ------------------ */
async function openPassModal(e) {
    const row = e.target.closest("tr");
    if (!row) return;

    const vehicleNo = row.children[0]?.textContent?.trim() || "";
    modalVehicleNo.textContent = vehicleNo;
    passDate.value = new Date().toISOString().split("T")[0];
    document.getElementById("totalPeople").value = 1;
    addPassBtn.dataset.vehicleNo = vehicleNo;

    checkpostSelect.innerHTML = '<option value="">Loading...</option>';
    try {
        const res = await fetch(`${checkApi}/GetAll`, {
            headers: {
                "Authorization": `Bearer ${localStorage.getItem("token")}`,
                "Content-Type": "application/json"
            }
        });
        const checkposts = await res.json();
        checkpostSelect.innerHTML = '<option value="">Select Checkpost</option>';
        checkposts.forEach(cp => {
            const option = document.createElement("option");
            option.value = cp.checkpostId;
            option.textContent = cp.checkpostName;
            checkpostSelect.appendChild(option);
        });
        if (checkposts.length === 0) {
            const opt = document.createElement("option");
            opt.value = "";
            opt.textContent = "No checkposts found";
            checkpostSelect.appendChild(opt);
        }
    } catch (err) {
        console.error("Error loading checkposts:", err);
        checkpostSelect.innerHTML = '<option value="">Error loading checkposts</option>';
    }

    passModal.style.display = "flex";
}

/* ------------------ Add Pass ------------------ */
/* ------------------ Add Pass ------------------ */
addPassBtn.addEventListener("click", async () => {
    const vehicleNo = addPassBtn.dataset.vehicleNo?.trim();
    const checkpostId = parseInt(checkpostSelect.value);
    const totalPeople = parseInt(document.getElementById("totalPeople").value) || 0;

    // ✅ Pass is always true (no selection)
    const pass = true;

    if (!vehicleNo) return alert("Vehicle number missing.");
    if (!checkpostId) return alert("Select a checkpost.");
    if (!totalPeople || totalPeople <= 0) return alert("Enter valid total people count.");

    const payload = { 
        VehicleNo: vehicleNo, 
        CheckpostId: checkpostId, 
        Pass: pass,                   // ✅ Always true
        TotalPeople: totalPeople 
        // ✅ No date field needed — backend will assign DateTime.Now
    };

    try {
        const res = await fetch(`${apiBaseUrl}/Add`, {
            method: "POST",
            headers: { 
                "Authorization": `Bearer ${localStorage.getItem("token")}`, 
                "Content-Type": "application/json" 
            },
            body: JSON.stringify(payload)
        });

        const text = await res.text();
        if (!res.ok) {
            if (text.toLowerCase().includes("duplicate") || text.toLowerCase().includes("unique")) {
                return alert("⚠️ Duplicate entry: This vehicle already has a pass.");
            } else {
                return alert("Error adding pass.");
            }
        }

        alert(JSON.parse(text)?.message || "✅ Pass added successfully!");
        passModal.style.display = "none";
        searchVehicle();
        loadPassedVehicles();
    } catch (err) {
        console.error(err);
        alert("Error adding pass.");
    }
});


/* ------------------ Close Pass Modal ------------------ */
cancelPassBtn.addEventListener("click", () => passModal.style.display = "none");
closePassModalBtn.addEventListener("click", () => passModal.style.display = "none");

/* ------------------ Passed Vehicles Modal ------------------ */
passedVehicleBtn.addEventListener("click", async () => {
    passedListModal.style.display = "flex";
    await loadPassedVehicles();
});
closePassedListModalBtn.addEventListener("click", () => passedListModal.style.display = "none");

/* ------------------ Load Passed Vehicles ------------------ */
async function loadPassedVehicles() {
    try {
        const res = await fetch(`${apiBaseUrl}/GetAll`, {
            headers: { "Authorization": `Bearer ${localStorage.getItem("token")}` }
        });
        if (!res.ok) throw new Error(`API Error: ${res.status}`);
        const data = await res.json();
        displayPassedVehicles(data);
    } catch (err) {
        console.error(err);
        passedListTableBody.innerHTML = `<tr><td colspan="6" style="text-align:center;">Error loading data</td></tr>`;
    }
}

/* ------------------ Display Passed Vehicles ------------------ */
function displayPassedVehicles(data) {
    passedListTableBody.innerHTML = ""; // Clear previous rows

    if (!data || data.length === 0) {
        passedListTableBody.innerHTML = `<tr><td colspan="7" style="text-align:center;">No passed vehicles found</td></tr>`;
        passedVehicleCount = 0;
        updateCounts();
        return;
    }

    passedVehicleCount = data.length;
    updateCounts();

    data.forEach(v => {
        const row = document.createElement("tr");

        // ✅ Format date + time together
        let formattedDateTime = "-";
        if (v.currentDate) {
            const date = new Date(v.currentDate);
            const options = { 
                year: "numeric", 
                month: "short", 
                day: "2-digit", 
                hour: "2-digit", 
                minute: "2-digit", 
                hour12: true 
            };
            formattedDateTime = date.toLocaleString(undefined, options);
        }

        row.innerHTML = `
            <td>${v.vehicleNo || "-"}</td>
            <td>${v.checkpostName || "-"}</td>
            <td>${v.pass ? "Yes" : "No"}</td>
            <td>${v.totalPeople || "-"}</td>
            <td>${v.vehicleNodalName || "-"}</td>
            <td>${v.nodalMobileNo || "-"}</td>
            <td>${formattedDateTime}</td>
        `;

        passedListTableBody.appendChild(row);
    });
}


/* ------------------ Add New Checkpost ------------------ */
addCheckpostBtn.addEventListener("click", () => newCheckpostDiv.style.display = "block");
saveCheckpostBtn.addEventListener("click", async () => {
    const newName = newCheckpostInput.value.trim();
    if (!newName) return alert("Enter checkpost name");
    try {
        const res = await fetch(`${checkApi}/Add`, {
            method: "POST",
            headers: { "Authorization": `Bearer ${localStorage.getItem("token")}`, "Content-Type": "application/json" },
            body: JSON.stringify({ checkpostname1: newName })
        });
        if (!res.ok) throw new Error("Add failed");
        alert("Checkpost added successfully!");
        const option = document.createElement("option");
        option.value = new Date().getTime();
        option.textContent = newName;
        checkpostSelect.appendChild(option);
        checkpostSelect.value = option.value;
        newCheckpostInput.value = "";
        newCheckpostDiv.style.display = "none";
    } catch (err) {
        console.error(err);
        alert("Error adding checkpost");
    }
});
const cancelPassedListBtn = document.getElementById("cancelPassedListBtn");
cancelPassedListBtn.addEventListener("click", () => {
    passedListModal.style.display = "none";
});

// ------------------ Search Passed Vehicles ------------------
 
const searchPassedVehiclesInput = document.getElementById("searchPassedVehicles");

let passedSearchTimeout;
searchPassedVehiclesInput.addEventListener("input", () => {
    clearTimeout(passedSearchTimeout);
    passedSearchTimeout = setTimeout(searchPassedVehicles, 300); // 300ms debounce
});

async function searchPassedVehicles() {
    const vehicleNo = searchPassedVehiclesInput.value.trim().toLowerCase();

    try {
        const res = await fetch(`${apiBaseUrl}/GetAll`, { // fetch all passed vehicles
            headers: { "Authorization": `Bearer ${localStorage.getItem("token")}` }
        });

        if (!res.ok) throw new Error(`API Error: ${res.status}`);
        const data = await res.json();

        const filtered = data.filter(v => {
            return vehicleNo ? v.vehicleNo.toLowerCase().includes(vehicleNo) : true;
        });

        displayPassedVehicles(filtered);
    } catch (err) {
        console.error("Error searching passed vehicles:", err);
        passedListTableBody.innerHTML = `<tr><td colspan="7" style="text-align:center;">Error fetching vehicles</td></tr>`;
    }
}



/* ------------------ Initial Load ------------------ */
loadDistricts();
loadAllVehicles();
loadPassedVehicles();

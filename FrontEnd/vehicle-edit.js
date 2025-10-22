const apiUrl = "https://localhost:7005/api/Vehicleregistration";
const blockApiUrl = "https://localhost:7005/api/Block";
const token = localStorage.getItem("token");
const district = localStorage.getItem("district");
const vehicleNo = localStorage.getItem("editVehicleNo");

// Ensure user is logged in
if (!token) {
    alert("Please login first!");
    window.location.href = "login.html";
}

// Display district
document.getElementById("districtDisplay").textContent = `District: ${district}`;

// Back button
document.getElementById("backBtn").addEventListener("click", () => {
    window.location.href = "VehicleView.html";
});

// Load blocks dropdown
async function loadBlocks(selectedBlock) {
    try {
        const response = await fetch(`${blockApiUrl}/GetAll`, {
            headers: { "Authorization": `Bearer ${token}` }
        });
        const blocks = await response.json();
        const dropdown = document.getElementById("BlockDropdown");
        dropdown.innerHTML = '';

        blocks
            .filter(b => (b.DistrictName || b.districtName).toLowerCase() === district.toLowerCase())
            .forEach(b => {
                const name = b.Blockname || b.blockname;
                const option = document.createElement("option");
                option.value = name;
                option.textContent = name;
                if (name === selectedBlock) option.selected = true;
                dropdown.appendChild(option);
            });
    } catch (err) {
        console.error("Error loading blocks:", err);
    }
}

// Load vehicle data by vehicleNo
async function loadVehicle() {
    if (!vehicleNo) {
        alert("No vehicle selected for editing!");
        window.location.href = "VehicleView.html";
        return;
    }
 
     try {
            const response = await fetch(`${apiUrl}/search/${vehicleNo}`, {
                method: "GET",
                headers: {
                    "Authorization": `Bearer ${token}`,
                    "Content-Type": "application/json"
                }
            });
 
            if (!response.ok) throw new Error(await response.text());
 
        const vehicles = await response.json();
        if (!vehicles || vehicles.length === 0) throw new Error("Vehicle not found");

        const vehicle = vehicles[0]; // take the first match

        // Fill form fields
        document.getElementById("VehicleNo").value = vehicle.vehicleNo; // read-only
        document.getElementById("VehicleNo").setAttribute("readonly", true);
        document.getElementById("VehicleType").value = vehicle.vehicleType || '';
        document.getElementById("SeatCapacity").value = vehicle.seatCapacity || '';
        document.getElementById("DriverName").value = vehicle.driverName || '';
        document.getElementById("DriverMobileNo").value = vehicle.driverMobileNo || '';
        document.getElementById("VehicleNodalName").value = vehicle.vehicleNodalName || '';
        document.getElementById("NodalMobileNo").value = vehicle.nodalMobileNo || '';
        document.getElementById("Gpname").value = vehicle.gpname || '';
        document.getElementById("Remark").value = vehicle.remark || '';

        await loadBlocks(vehicle.blockName || '');
    } catch (err) {
        console.error("Load vehicle error:", err);
        alert("Failed to load vehicle. " + err.message);
    }
}

loadVehicle();

// Update vehicle
// -------------------- FORM INPUTS --------------------
const vehicleTypeInput = document.getElementById("VehicleType");
const seatCapacityInput = document.getElementById("SeatCapacity");
const driverNameInput = document.getElementById("DriverName");
const driverMobileInput = document.getElementById("DriverMobileNo");
const vehicleNodalInput = document.getElementById("VehicleNodalName");
const nodalMobileInput = document.getElementById("NodalMobileNo");
 
const blockDropdownInput = document.getElementById("BlockDropdown");
const gpNameInput = document.getElementById("Gpname");
const remarkInput = document.getElementById("Remark");

const updateBtn = document.getElementById("updateBtn");

// Replace with your actual backend base URL
 

updateBtn.addEventListener("click", async (e) => {
    e.preventDefault(); 
	// -------------------- FETCH STORED VEHICLE NO --------------------
        const vehicleNo = localStorage.getItem("editVehicleNo");
    if (!vehicleNo) return alert("Vehicle number not found.");
 
    // -------------------- FETCH AUTH TOKEN --------------------
    const token = localStorage.getItem("token");
    if (!token) {
        alert("❌ Please login first!");
        return;
    }
 
    // -------------------- PREPARE UPDATED DATA --------------------
   const updatedData = {
	    VehicleNo: vehicleNo,
    VehicleType: document.getElementById("VehicleType")?.value || "",
    SeatCapacity: parseInt(document.getElementById("SeatCapacity")?.value) || null,
    DriverName: document.getElementById("DriverName")?.value || "",
    DriverMobileNo: document.getElementById("DriverMobileNo")?.value || "",
    VehicleNodalName: document.getElementById("VehicleNodalName")?.value || "",
    NodalMobileNo: document.getElementById("NodalMobileNo")?.value || "",
     District: district,  
    BlockName: document.getElementById("BlockDropdown")?.value || "",
    Gpname: document.getElementById("Gpname")?.value || "",
    Remark: document.getElementById("Remark")?.value || ""
};
 

    // -------------------- API REQUEST FROM HERE I M GETTING ERROR UPER ALL CASE SOLVE --------------------
  try {
    const response = await fetch(`${apiUrl}/UpdateByVehicleNo/${vehicleNo}`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json",
            "Authorization": `Bearer ${token}`
        },
        body: JSON.stringify(updatedData)
    });
	  // Read response content safely
     let result;
    try {
        result = await response.json();
    } catch {
        result = await response.text();
    }

    console.log("Response status:", response.status);
    console.log("Response body:", result);

    if (!response.ok) {
        // Convert object to readable message
        const message =
            typeof result === "object"
                ? result.message || result.Message || JSON.stringify(result)
                : result || "Update failed!";
        throw new Error(message);
    }

   const successMessage =
        typeof result === "object"
            ? result.message || result.Message || "Vehicle updated successfully!"
            : result;

    alert("✅ " + successMessage);
    window.location.href = "VehicleView.html";

} catch (error) {
    console.error("❌ Update error:", error);
    alert("❌ " + error.message);
}
});


// Delete vehicle
 
const deleteBtn = document.getElementById("deleteBtn");

deleteBtn.addEventListener("click", async () => {
    const vehicleNo = localStorage.getItem("editVehicleNo");
    if (!vehicleNo) return alert("Vehicle number not found.");

    const token = localStorage.getItem("token");

    try {
        const response = await fetch(`${apiUrl}/DeleteByVehicleNo/${vehicleNo}`, {
            method: "DELETE",
            headers: { "Authorization": `Bearer ${token}` }
        });

        const text = await response.text();

        if (!response.ok) {
            // Show backend exception message
            alert(`❌ ${text}`);
            return;
        }

        alert(`✅ Vehicle ${vehicleNo} deleted successfully.`);
        window.location.href = "VehicleView.html"; // redirect back

    } catch (error) {
        console.error("Delete error:", error);
        alert("❌ Error deleting vehicle: " + error.message);
    }
});



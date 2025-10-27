const apiUrl = "/api/Vehicleregistration";
const blockApiUrl = "/api/Block";

// Display logged-in district
window.addEventListener("DOMContentLoaded", () => {
	const userDistrictElem = document.getElementById("userDistrict");
	const district = localStorage.getItem("district") || "Unknown";
	userDistrictElem.textContent = `District: ${district}`;
});

async function loadBlocks() {
	const district = localStorage.getItem("district");
	if (!district) return;

	try {
		const response = await fetch(`${blockApiUrl}/GetAll`, {
			headers: { Authorization: "Bearer " + localStorage.getItem("token") },
		});
		const blocks = await response.json();
		console.log("🔍 Blocks from API:", blocks);

		const blockDropdown = document.getElementById("BlockDropdown");
		blockDropdown.innerHTML = '<option value="">Select Block</option>';

		// Match both DistrictName and districtName
		const filteredBlocks = blocks.filter((b) => {
			const name = b.DistrictName || b.districtName;
			return name && name.toLowerCase() === district.toLowerCase();
		});

		if (filteredBlocks.length === 0) {
			console.warn("⚠️ No blocks found for district:", district);
		}

		filteredBlocks.forEach((block) => {
			const name = block.Blockname || block.blockname;
			const option = document.createElement("option");
			option.value = name;
			option.textContent = name;
			blockDropdown.appendChild(option);
		});
	} catch (err) {
		console.error("❌ Error loading blocks:", err);
	}
}

// Add new block
/*async function addBlock() {
    const district = localStorage.getItem("district");
    const blockName = prompt("Enter new Block Name:");
    if (!blockName) return;

    try {
        const response = await fetch(`${blockApiUrl}/Add`, {
            method: "POST",
            headers: { 
                "Content-Type": "application/json",
                "Authorization": "Bearer " + localStorage.getItem("token")
            },
            body: JSON.stringify({ Blockname: blockName, DistrictName: district })
        });

        if (!response.ok) throw new Error(await response.text());
        alert("Block added successfully!");
        await loadBlocks();
    } catch (err) {
        alert("Error adding block: " + err.message);
    }
}*/

// Event listeners
window.addEventListener("DOMContentLoaded", () => {
	const userDistrictElem = document.getElementById("userDistrict");
	const district = localStorage.getItem("district") || "Unknown";
	userDistrictElem.textContent = `District: ${district}`;

	loadBlocks(); // load block dropdown
});

/*document.getElementById("AddBlockBtn").addEventListener("click", addBlock);*/

// Add Vehicle
document.getElementById("vehicleForm").addEventListener("submit", function (e) {
	e.preventDefault();

	const district = localStorage.getItem("district") || "Unknown";

	const vehicle = {
		VehicleNo: document.getElementById("VehicleNo").value,
		VehicleType: document.getElementById("VehicleType").value,
		SeatCapacity:
			parseInt(document.getElementById("SeatCapacity").value) || null,
		DriverName: document.getElementById("DriverName").value,
		DriverMobileNo: document.getElementById("DriverMobileNo").value,
		VehicleNodalName: document.getElementById("VehicleNodalName").value,
		NodalMobileNo: document.getElementById("NodalMobileNo").value,
		District: district, // ✅ auto from login
		BlockName: document.getElementById("BlockDropdown").value,
		Gpname: document.getElementById("Gpname").value,
		Remark: document.getElementById("Remark").value,
	};

	fetch(`${apiUrl}/Add`, {
		method: "POST",
		headers: {
			"Content-Type": "application/json",
			Authorization: "Bearer " + localStorage.getItem("token"),
		},
		body: JSON.stringify(vehicle),
	})
		.then((res) => {
			if (!res.ok) throw new Error("Failed to add vehicle");
			return res.text();
		})
		.then((message) => {
			alert("✅ Vehicle added successfully!");
			document.getElementById("vehicleForm").reset();
		})
		.catch((err) => {
			alert("❌ Error: " + err.message);
		});
});

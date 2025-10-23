const apiUrl = "/api/Vehicleregistration";
const blockApiUrl = "/api/Block";
const exportApiUrl = "/api/Vehicleregistration/ExportToExcel";

window.addEventListener("DOMContentLoaded", async () => {
	const district = localStorage.getItem("district");
	const token = localStorage.getItem("token");

	if (!token) {
		alert("Please login first!");
		window.location.href = "login.html";
		return;
	}

	document.getElementById("districtDisplay").textContent = district
		? `District: ${district}`
		: "District: Unknown";

	// Load blocks and vehicles initially
	const blocks = await loadBlocks();
	await loadAllVehicles();

	// ==================== Insert Block Dropdown in Table Header ====================
	const blockHeaderDropdown = document.createElement("select");
	blockHeaderDropdown.id = "blockHeaderDropdown";
	blockHeaderDropdown.innerHTML = '<option value="">All Blocks</option>';

	const blockTh = document.querySelector("#vehicleTable th:nth-child(9)"); // Block column header
	blockTh.innerHTML = "Block";
	blockTh.appendChild(blockHeaderDropdown);

	blocks
		.filter(
			(b) =>
				(b.DistrictName || b.districtName).toLowerCase() ===
				district.toLowerCase()
		)
		.forEach((b) => {
			const name = b.Blockname || b.blockname;
			const option = document.createElement("option");
			option.value = name;
			option.textContent = name;
			blockHeaderDropdown.appendChild(option);
		});

	// Filter table rows when dropdown changes
	blockHeaderDropdown.addEventListener("change", (e) => {
		const selectedBlock = e.target.value;
		const tbody = document.querySelector("#vehicleTable tbody");
		const rows = Array.from(tbody.querySelectorAll("tr"));

		rows.forEach((row) => {
			const blockCell = row.children[8]; // Block column = 9th column
			if (!selectedBlock || blockCell.textContent === selectedBlock) {
				row.style.display = "";
			} else {
				row.style.display = "none";
			}
		});
	});

	// ==================== Buttons ====================
	document.getElementById("backBtn").addEventListener("click", () => {
		window.location.href = "home.html";
	});

	document.getElementById("searchBtn").addEventListener("click", searchVehicle);
	document.getElementById("resetBtn").addEventListener("click", async () => {
		document.getElementById("searchInput").value = "";
		const blockHeaderDropdown = document.getElementById("blockHeaderDropdown");
		if (blockHeaderDropdown) blockHeaderDropdown.value = "";
		await loadAllVehicles();
	});

	// ==================== EXPORT TO EXCEL ====================
	// ==================== EXPORT TO EXCEL ====================
	const exportBtn = document.getElementById("exportExcelBtn");

	if (exportBtn) {
		exportBtn.addEventListener("click", async () => {
			try {
				const table = document.querySelector("#vehicleTable");
				if (!table) {
					alert("Table not found!");
					return;
				}

				const headers = Array.from(table.querySelectorAll("thead th")).map(
					(th) => th.textContent.trim()
				);
				const rows = Array.from(table.querySelectorAll("tbody tr")).filter(
					(tr) => tr.style.display !== "none"
				); // only visible rows

				const data = rows.map((tr) => {
					const cells = Array.from(tr.querySelectorAll("td"));
					const obj = {};

					cells.forEach((cell, index) => {
						// Take normal text content
						obj[headers[index]] = cell.textContent.trim();
					});

					// Force column 8 (index 7) to be used for BlockName
					if (cells[8]) {
						obj["Block Name"] = cells[8].textContent.trim();
					}

					return obj;
				});

				// Create payload for backend
				const payload = data.map((v) => ({
					VehicleNo: v["Vehicle No"] || "",
					VehicleType: v["Type"] || "",
					SeatCapacity: v["Seat Capacity"] || "",
					DriverName: v["Driver Name"] || "",
					DriverMobileNo: v["Driver Mobile"] || "",
					VehicleNodalName: v["Nodal Name"] || "",
					NodalMobileNo: v["Nodal Mobile"] || "",
					District: v["District"] || "",
					BlockName: v["Block Name"] || "", // directly from column 8
					Gpname: v["GP Name"] || "",
					Remark: v["Remark"] || "",
				}));

				const token = localStorage.getItem("token");

				const response = await fetch(exportApiUrl, {
					method: "POST",
					headers: {
						"Content-Type": "application/json",
						Authorization: `Bearer ${token}`,
					},
					body: JSON.stringify(payload),
				});

				if (!response.ok) {
					alert("❌ Failed to export data");
					return;
				}

				const blob = await response.blob();
				const url = window.URL.createObjectURL(blob);
				const a = document.createElement("a");
				a.href = url;
				a.download = "Vehicle_List.xlsx";
				document.body.appendChild(a);
				a.click();
				a.remove();
				window.URL.revokeObjectURL(url);
			} catch (err) {
				console.error("Export error:", err);
				alert("Export failed: " + err.message);
			}
		});
	}

	// ==================== LOAD BLOCKS ====================
	async function loadBlocks() {
		try {
			const token = localStorage.getItem("token");
			const response = await fetch(`${blockApiUrl}/GetAll`, {
				headers: { Authorization: `Bearer ${token}` },
			});
			const blocksData = await response.json();
			return blocksData;
		} catch (err) {
			console.error("Error loading blocks:", err);
			return [];
		}
	}

	// ==================== Load All Vehicles ====================
	async function loadAllVehicles() {
		try {
			const response = await fetch(apiUrl, {
				method: "GET",
				headers: {
					Authorization: `Bearer ${token}`,
					"Content-Type": "application/json",
				},
			});
			if (!response.ok) throw new Error(await response.text());
			const data = await response.json();
			populateTable(data);
		} catch (err) {
			console.error("Load all error:", err);
			alert("Failed to load vehicle data. " + err.message);
		}
	}

	// ==================== Search Vehicle ====================
	async function searchVehicle() {
		const vehicleNo = document.getElementById("searchInput").value.trim();
		if (!vehicleNo) {
			alert("Please enter a vehicle number or part of it.");
			return;
		}

		try {
			const response = await fetch(`${apiUrl}/search/${vehicleNo}`, {
				method: "GET",
				headers: {
					Authorization: `Bearer ${token}`,
					"Content-Type": "application/json",
				},
			});

			if (!response.ok) throw new Error(await response.text());

			const vehicle = await response.json();
			populateTable(Array.isArray(vehicle) ? vehicle : [vehicle]);
		} catch (err) {
			console.error("Search error:", err);
			alert("Search failed. " + err.message);
		}
	}

	// ==================== Populate Table ====================
	function populateTable(data) {
		const tbody = document.querySelector("#vehicleTable tbody");
		tbody.innerHTML = "";

		if (!data || data.length === 0) {
			tbody.innerHTML = `<tr><td colspan="12">No records found</td></tr>`;
			return;
		}

		data.forEach((v) => {
			let vehicleNoClass = "";
			const vehicleType = v.vehicleType.toLowerCase();
			const seatCapacity = v.seatCapacity || 0;

			if (
				(vehicleType === "bus" && seatCapacity >= 50) ||
				(vehicleType === "small vehicle" && seatCapacity >= 10)
			) {
				vehicleNoClass = "highlight-cell";
			}

			const row = document.createElement("tr");
			row.innerHTML = `
                <td class="${vehicleNoClass}">${v.vehicleNo}</td>
                <td>${v.vehicleType}</td>
                <td>${v.seatCapacity}</td>
                <td>${v.driverName}</td>
                <td>${v.driverMobileNo}</td>
                <td>${v.vehicleNodalName}</td>
                <td>${v.nodalMobileNo}</td>
                <td>${v.district}</td>
                <td>${v.blockName}</td>
                <td>${v.gpname}</td>
                <td>${v.remark}</td>
                <td>
                    <button class="edit-btn" data-vehicle="${v.vehicleNo}">Edit</button>
                </td>
            `;
			tbody.appendChild(row);
		});
		function onEditClick(vehicleNo) {
			localStorage.setItem("editVehicleNo", vehicleNo);
			window.location.href = "vehicle-edit.html";
		}
		document.querySelectorAll(".edit-btn").forEach((btn) => {
			btn.addEventListener("click", () => {
				const vehicleNo = btn.getAttribute("data-vehicle");
				localStorage.setItem("editVehicleNo", vehicleNo);
				window.location.href = "vehicle-edit.html";
			});
		});
	}
});

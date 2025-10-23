// API base URL
const apiBaseUrl = "/api/Source";

// DOM elements
const container = document.getElementById("container"); // main container div

// Create Source Pass Entry button
const sourcePassEntryBtn = document.createElement("button");

// Event: on button click, show search bar
sourcePassBtn.addEventListener("click", () => {
	// Clear container
	container.innerHTML = "";

	// Create search input
	const searchDiv = document.createElement("div");
	searchDiv.style.marginBottom = "10px";

	const input = document.createElement("input");
	input.type = "text";
	input.placeholder = "Enter Vehicle No";
	input.id = "vehicleNoInput";
	input.style.padding = "5px";

	const searchBtn = document.createElement("button");
	searchBtn.textContent = "Search";
	searchBtn.style.marginLeft = "5px";

	searchDiv.appendChild(input);
	searchDiv.appendChild(searchBtn);
	container.appendChild(searchDiv);

	// Table container
	const tableDiv = document.createElement("div");
	container.appendChild(tableDiv);

	// Search button click event
	searchBtn.addEventListener("click", async () => {
		const vehicleNo = input.value.trim();
		if (!vehicleNo) {
			alert("Please enter vehicle number");
			return;
		}

		try {
			const response = await fetch(
				`${apiBaseUrl}/Vehicle/SearchByVehicleNo?vehicleNo=${vehicleNo}`
			);
			if (!response.ok) throw new Error("Vehicle not found");

			const vehicle = await response.json();
			if (!vehicle) {
				tableDiv.innerHTML = "<p>No vehicle found</p>";
				return;
			}

			// Create table
			tableDiv.innerHTML = ""; // clear previous
			const table = document.createElement("table");
			table.border = "1";
			table.style.width = "100%";
			table.style.borderCollapse = "collapse";

			// Table header
			const thead = document.createElement("thead");
			thead.innerHTML = `
                <tr>
                    <th>Vehicle No</th>
                    <th>Vehicle Type</th>
                    <th>Driver Name</th>
                    <th>Seat Capacity</th>
                    <th>District</th>
                    <th>Block</th>
                    <th>Action</th>
                </tr>
            `;
			table.appendChild(thead);

			// Table body
			const tbody = document.createElement("tbody");
			const tr = document.createElement("tr");

			tr.innerHTML = `
                <td>${vehicle.vehicleNo}</td>
                <td>${vehicle.vehicleType}</td>
                <td>${vehicle.driverName}</td>
                <td>${vehicle.seatCapacity}</td>
                <td>${vehicle.districtName || ""}</td>
                <td>${vehicle.blockName || ""}</td>
                <td><button class="editBtn">Edit</button></td>
            `;

			tbody.appendChild(tr);
			table.appendChild(tbody);
			tableDiv.appendChild(table);

			// Edit button event
			tr.querySelector(".editBtn").addEventListener("click", () => {
				alert(`Edit Vehicle: ${vehicle.vehicleNo}`);
				// You can redirect to edit page or open a modal here
			});
		} catch (err) {
			alert(err.message);
		}
	});
});

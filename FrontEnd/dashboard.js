// API URLs
const vehicleApiUrl = "/api/Dashboard/GetByDistrict"; // Vehicle dashboard
const checkpostApiUrl = "/api/CheckpostDashboard"; // Checkpost
const foodApiUrl = "/api/DistrictDashboard"; // districtsource

// Hold data in memory
let vehicleData = null;
let checkpostData = null;
let foodData = null;

// On page load
window.addEventListener("DOMContentLoaded", () => {
	loadVehicleDashboard();
	loadCheckpostDashboard();
	loadFoodDashboard();
});

// Logout
document.getElementById("logoutBtn").addEventListener("click", () => {
	localStorage.clear();
	window.location.href = "login.html";
});

// ------------------- Vehicle Dashboard -------------------
async function loadVehicleDashboard() {
	const token = localStorage.getItem("token");

	try {
		const response = await fetch(vehicleApiUrl, {
			headers: { Authorization: "Bearer " + token },
		});

		if (!response.ok) throw new Error("Failed to fetch vehicle dashboard");

		vehicleData = await response.json();

		// ✅ Show overall total capacity (not "people")
		// ✅ Show overall total capacity and display table
		if (Array.isArray(vehicleData)) {
			// Backend returned plain list
			let totalCapacity = vehicleData.reduce(
				(sum, d) => sum + (d.totalCapacityPerDistrict || 0),
				0
			);
			document.getElementById("vehicleOverallCapacity").textContent =
				totalCapacity.toLocaleString();
			displayVehicleTable(vehicleData);
		} else if (vehicleData.districtData) {
			// Backend returned wrapped object
			document.getElementById("vehicleOverallCapacity").textContent =
				vehicleData.overallCapacity.toLocaleString();
			displayVehicleTable(vehicleData.districtData);
		}

		// ✅ Update top cards
		updateTopCards();
	} catch (error) {
		console.error("Vehicle Dashboard error:", error);
	}
}

function displayVehicleTable(data) {
	const tbody = document.getElementById("vehicleDashboardBody");
	tbody.innerHTML = "";

	let totalBuses = 0;
	let totalSmallVehicles = 0;
	let totalBusCapacity = 0;
	let totalSmallVehicleCapacity = 0;
	let totalCapacity = 0;

	data.forEach((d) => {
		tbody.innerHTML += `
            <tr>
                <td>${d.districtName}</td>
                <td>${d.blockName}</td>
                <td>${d.numberOfBuses}</td>
                <td>${d.totalBusCapacity}</td>
                <td>${d.numberOfSmallVehicles}</td>
                <td>${d.totalSmallVehicleCapacity}</td>
                <td>${d.totalCapacityPerDistrict}</td>
            </tr>
        `;

		totalBuses += Number(d.numberOfBuses) || 0;
		totalSmallVehicles += Number(d.numberOfSmallVehicles) || 0;
		totalBusCapacity += Number(d.totalBusCapacity) || 0;
		totalSmallVehicleCapacity += Number(d.totalSmallVehicleCapacity) || 0;
		totalCapacity += Number(d.totalCapacityPerDistrict) || 0;
	});

	// ✅ Totals row
	tbody.innerHTML += `
        <tr style="font-weight:bold; background:#EAE4D5;">
            <td colspan="2">Total</td>
            <td>${totalBuses}</td>
            <td>${totalBusCapacity}</td>
            <td>${totalSmallVehicles}</td>
            <td>${totalSmallVehicleCapacity}</td>
            <td>${totalCapacity}</td>
        </tr>
    `;
}

// ------------------- Checkpost Dashboard -------------------
// ------------------- Checkpost Dashboard -------------------
async function loadCheckpostDashboard() {
	const token = localStorage.getItem("token");
	try {
		const response = await fetch(checkpostApiUrl, {
			headers: { Authorization: "Bearer " + token },
		});
		if (!response.ok) throw new Error("Failed to fetch checkpost dashboard");

		// Expecting { overallPeople, districtData } or direct list depending on backend
		checkpostData = await response.json();

		// ✅ If backend returns a plain list (not wrapped object), compute overall manually
		if (Array.isArray(checkpostData)) {
			const totalOverall = checkpostData.reduce(
				(sum, d) => sum + (d.totalPeopleInDistrict || 0),
				0
			);
			document.getElementById("checkpostOverallPeople").textContent =
				totalOverall.toLocaleString();
			displayCheckpostTable(checkpostData);
		}
		// ✅ If backend returns wrapped JSON { overallPeople, districtData }
		else if (checkpostData.districtData) {
			document.getElementById("checkpostOverallPeople").textContent =
				checkpostData.overallPeople.toLocaleString();
			displayCheckpostTable(checkpostData.districtData);
		}

		updateTopCards();
	} catch (error) {
		console.error("Checkpost Dashboard error:", error);
	}
}

function displayCheckpostTable(data) {
	const tbody = document.getElementById("checkpostDashboardBody");
	tbody.innerHTML = "";

	let totalBuses = 0;
	let totalSmallVehicles = 0;
	let totalBusCapacity = 0;
	let totalSmallVehicleCapacity = 0;
	let totalCapacity = 0;

	data.forEach((d) => {
		tbody.innerHTML += `
            <tr>
                <td>${d.district || "—"}</td>
                <td>${d.blockName || "—"}</td>
                <td>${d.noOfBuses}</td>
                <td>${d.totalPeopleInBuses}</td>
                <td>${d.noOfSmallVehicles}</td>
                <td>${d.totalPeopleSmallVehicles}</td>
                <td>${d.totalPeopleInDistrict}</td>
            </tr>`;

		totalBuses += Number(d.noOfBuses) || 0;
		totalSmallVehicles += Number(d.noOfSmallVehicles) || 0;
		totalBusCapacity += Number(d.totalPeopleInBuses) || 0;
		totalSmallVehicleCapacity += Number(d.totalPeopleSmallVehicles) || 0;
		totalCapacity += Number(d.totalPeopleInDistrict) || 0;
	});

	// Add totals row
	tbody.innerHTML += `
        <tr style="font-weight:bold; background:#EAE4D5;">
            <td colspan="2">Total</td>
            <td>${totalBuses}</td>
            <td>${totalBusCapacity}</td>
            <td>${totalSmallVehicles}</td>
            <td>${totalSmallVehicleCapacity}</td>
            <td>${totalCapacity}</td>
        </tr>`;
}

// ------------------- Food Status Dashboard -------------------
async function loadFoodDashboard() {
	const token = localStorage.getItem("token");
	try {
		const response = await fetch(foodApiUrl, {
			headers: { Authorization: "Bearer " + token },
		});
		if (!response.ok) throw new Error("Failed to fetch food dashboard");

		foodData = await response.json();

		if (Array.isArray(foodData)) {
			// Backend returned plain list
			let totalPeople = foodData.reduce(
				(sum, d) => sum + (d.totalPeopleInDistrict || 0),
				0
			);
			document.getElementById("foodOverallPeople").textContent =
				totalPeople.toLocaleString();
			displayFoodTable(foodData);
		} else if (foodData.districtData) {
			// Backend returned wrapped object
			document.getElementById("foodOverallPeople").textContent =
				foodData.overallPeople.toLocaleString();
			displayFoodTable(foodData.districtData);
		}

		// ✅ Update top cards
		updateTopCards();
	} catch (error) {
		console.error("Food Dashboard error:", error);
	}
}

function displayFoodTable(data) {
	const tbody = document.getElementById("foodDashboardBody");
	tbody.innerHTML = "";

	let totalBuses = 0;
	let totalSmallVehicles = 0;
	let totalPeopleInBuses = 0;
	let totalPeopleSmallVehicles = 0;
	let totalPeople = 0;

	data.forEach((d) => {
		tbody.innerHTML += `
            <tr>
                <td>${d.district}</td>
                <td>${d.blockName}</td>
                <td>${d.noOfBuses}</td>
                <td>${d.totalPeopleInBuses}</td>
                <td>${d.noOfSmallVehicles}</td>
                <td>${d.totalPeopleSmallVehicles}</td>
                <td>${d.totalPeopleInDistrict}</td>
            </tr>`;

		totalBuses += Number(d.noOfBuses) || 0;
		totalSmallVehicles += Number(d.noOfSmallVehicles) || 0;
		totalPeopleInBuses += Number(d.totalPeopleInBuses) || 0;
		totalPeopleSmallVehicles += Number(d.totalPeopleSmallVehicles) || 0;
		totalPeople += Number(d.totalPeopleInDistrict) || 0;
	});

	// Add totals row
	tbody.innerHTML += `
        <tr style="font-weight:bold; background:#EAE4D5;">
            <td colspan="2">Total</td>
            <td>${totalBuses}</td>
            <td>${totalPeopleInBuses}</td>
            <td>${totalSmallVehicles}</td>
            <td>${totalPeopleSmallVehicles}</td>
            <td>${totalPeople}</td>
        </tr>`;
}

// Update top cards
function updateTopCards() {
	// Vehicle Overall Capacity
	if (vehicleData) {
		document.getElementById("vehicleOverallCapacity").textContent =
			vehicleData.overallCapacity.toLocaleString();
	}

	// Checkpost Total People
	if (checkpostData) {
		document.getElementById("checkpostTotalPeopleCard").textContent =
			checkpostData.overallPeople.toLocaleString();
	}

	// Food Total People
	if (foodData) {
		document.getElementById("foodOverallPeople").textContent =
			foodData.overallPeople.toLocaleString();
	}

	// Total Buses and Total Small Vehicles (from Vehicle Dashboard)
	if (vehicleData && vehicleData.districtData) {
		let totalBuses = 0,
			totalSmallVehicles = 0;
		vehicleData.districtData.forEach((d) => {
			totalBuses += Number(d.numberOfBuses) || 0;
			totalSmallVehicles += Number(d.numberOfSmallVehicles) || 0;
		});
		document.getElementById("totalBusesCard").textContent = totalBuses;
		document.getElementById("totalSmallVehiclesCard").textContent =
			totalSmallVehicles;
	}
}

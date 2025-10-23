const apiUrl = "/api/DistrictDashboard";

// On page load, fetch and display dashboard data
window.addEventListener("DOMContentLoaded", () => {
	loadDashboardData();
});

// Fetch dashboard data for all districts
async function loadDashboardData() {
	const token = localStorage.getItem("token");

	try {
		const response = await fetch(apiUrl, {
			headers: {
				Authorization: "Bearer " + token,
			},
		});

		if (!response.ok) throw new Error("Failed to fetch dashboard data");

		const result = await response.json();

		displayOverallPeople(result.overallPeople); // Matches service property
		displayDistrictData(result.districtData); // Matches service property
	} catch (error) {
		console.error("Error loading dashboard:", error);
		alert("Unable to load dashboard data.");
	}
}

// Display overall people
function displayOverallPeople(value) {
	const elem = document.getElementById("overallPeople");
	elem.textContent = value.toLocaleString();
}

// Display district-wise summary
function displayDistrictData(data) {
	const tbody = document.getElementById("dashboardBody");
	tbody.innerHTML = "";

	data.forEach((d) => {
		const row = `
            <tr>
                <td>${d.district}</td>
                <td>${d.noOfBuses}</td>
                <td>${d.totalPeopleInBuses}</td>
                <td>${d.noOfSmallVehicles}</td>
                <td>${d.totalPeopleSmallVehicles}</td>
                <td>${d.totalPeopleInDistrict}</td>
            </tr>`;
		tbody.innerHTML += row;
	});
}

// Logout function
document.getElementById("logoutBtn").addEventListener("click", () => {
	localStorage.clear();
	window.location.href = "login.html";
});

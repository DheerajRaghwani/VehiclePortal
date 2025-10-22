const form = document.getElementById("loginForm");
const errorMsg = document.getElementById("errorMsg");
let successMsg = document.getElementById("successMsg");
if (!successMsg) {
  successMsg = document.createElement("div");
  successMsg.id = "successMsg";
  successMsg.style.color = "green";
  successMsg.style.fontWeight = "600";
  successMsg.style.marginTop = "10px";
  form.appendChild(successMsg);
}

form.addEventListener("submit", async (e) => {
  e.preventDefault();

  const username = document.getElementById("username").value.trim();
  const password = document.getElementById("password").value.trim();

  if (!username || !password) {
    errorMsg.textContent = "Please enter username and password";
    return;
  }

  try {
    const response = await fetch("https://localhost:7005/api/Auth/Login", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ username, password })
    });

    const data = await response.json();

    if (!response.ok) {
      errorMsg.textContent = data.message || "Invalid credentials";
      return;
    }
	 

    // ✅ Save token
    if (data.token) localStorage.setItem("token", data.token);

    // ✅ Save district info (skip extraction for admin)
    let roleFromResponse = (data.role && typeof data.role === "string") ? data.role : null;
    let role = roleFromResponse ? roleFromResponse.toLowerCase().trim() : null;

    if (role === "admin") {
      localStorage.setItem("district", "All Districts");
      localStorage.setItem("role", "admin");
    } else {
      // Normal user: save district
      let districtFromResponse = (data.districtName && typeof data.districtName === "string") ? data.districtName : null;
      let district = districtFromResponse || extractDistrictFromJwt(data.token) || "Unknown";
      localStorage.setItem("district", district.trim());
      localStorage.setItem("role", role);
    }

    if (data.districtId) localStorage.setItem("districtId", data.districtId);
    if (data.districtName) localStorage.setItem("districtName", data.districtName);
	 
	 successMsg.textContent = "Login successful! Redirecting...";
    errorMsg.textContent = "";


    // ✅ Redirect based on role
    const routeMap = {
      admin: "dashboard.html",
      user: "home.html",
      manager: "manager.html",
	  checkpost: "checkpost.html",
	  source: "source.html"
    };

    const destination = routeMap[role] || "dashboard.html";
    window.location.href = destination;

  } catch (err) {
    console.error(err);
    errorMsg.textContent = "Server error. Try again later.";
  }
});

/**
 * Decode JWT and extract role
 */
function extractRoleFromJwt(token) {
  if (!token) return null;
  try {
    const payload = JSON.parse(base64UrlDecode(token.split(".")[1]));
    return payload.role || payload.roles || null;
  } catch { return null; }
}

/**
 * Decode JWT and extract district (optional)
 */
function extractDistrictFromJwt(token) {
  if (!token) return null;
  try {
    const payload = JSON.parse(base64UrlDecode(token.split(".")[1]));
    return payload.districtName || payload.district || null;
  } catch { return null; }
}

/**
 * Base64Url decode
 */
function base64UrlDecode(str) {
  let base64 = str.replace(/-/g, "+").replace(/_/g, "/");
  while (base64.length % 4) base64 += "=";
  try {
    return decodeURIComponent(Array.prototype.map.call(atob(base64), c => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2)).join(''));
  } catch {
    return atob(base64);
  }
}

document.getElementById('wifiForm').addEventListener('submit', function (e) {
  e.preventDefault();

  let ssid = document.getElementById('ssid').value;
  let password = document.getElementById('password').value;

  fetch('/wifi', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ ssid: ssid, password: password })
  })
  .then(response => response.text())
  .then(data => {
    console.log('Response from ESP32:', data);
    document.getElementById('response').textContent = data;
  })
  .catch((error) => {
    console.error('Error:', error);
  });
});

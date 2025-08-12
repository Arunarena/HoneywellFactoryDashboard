document.addEventListener('DOMContentLoaded', function() {
    let machineOutputChart, productionTrendChart;

    function showAlert(idleMachines) {
        const alertBox = document.getElementById('alertBox');
        if (idleMachines > 0) {
            alertBox.innerHTML = `<div class="alert alert-warning" role="alert">${idleMachines} machine(s) are idle!</div>`;
        } else {
            alertBox.innerHTML = '';
        }
    }

    function renderCharts(data) {
        // Machine Output Chart
        if (!machineOutputChart) {
            const ctx1 = document.getElementById('machineOutputChart').getContext('2d');
            machineOutputChart = new Chart(ctx1, {
                type: 'bar',
                data: {
                    labels: data.machineOutputs.map((_, i) => 'M' + (i + 1)),
                    datasets: [{
                        label: 'Output',
                        data: data.machineOutputs,
                        backgroundColor: '#007bff88',
                        borderColor: '#007bff',
                        borderWidth: 1
                    }]
                },
                options: {
                    scales: { y: { beginAtZero: true, max: 120 } }
                }
            });
        } else {
            machineOutputChart.data.datasets[0].data = data.machineOutputs;
            machineOutputChart.update();
        }

        // Production Trend Chart
        if (!productionTrendChart) {
            const ctx2 = document.getElementById('productionTrendChart').getContext('2d');
            productionTrendChart = new Chart(ctx2, {
                type: 'line',
                data: {
                    labels: data.productionTrend.map((_, i) => ''),
                    datasets: [{
                        label: 'Production Rate',
                        data: data.productionTrend,
                        fill: true,
                        backgroundColor: 'rgba(40,167,69,0.2)',
                        borderColor: '#28a745',
                        tension: 0.3
                    }]
                },
                options: {
                    scales: { y: { beginAtZero: true, max: 120 } }
                }
            });
        } else {
            productionTrendChart.data.datasets[0].data = data.productionTrend;
            productionTrendChart.update();
        }
    }

    function updateMetrics() {
        fetch('/api/status')
            .then(res => res.json())
            .then(data => {
                document.getElementById('machineCount').textContent = data.machineCount;
                document.getElementById('activeAlarms').textContent = data.activeAlarms;
                document.getElementById('productionRate').textContent = data.productionRate + '%';
                renderCharts(data);
                showAlert(data.idleMachines);
            });
    }
    updateMetrics();
    setInterval(updateMetrics, 2000);
});

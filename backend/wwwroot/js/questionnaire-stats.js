document.addEventListener("DOMContentLoaded", function () {
    // Initialize charts when the stats tab is shown
    document.querySelector('a[href="#stats"]').addEventListener('shown.bs.tab', function () {
        initializeCharts();
    });
});

function initializeCharts() {
    // Radio Button Question Chart
    if (document.getElementById("radioChart")) {
        new Chart(document.getElementById("radioChart"), {
            type: 'bar',
            data: {
                labels: ["Red", "Blue", "Green"],
                datasets: [{
                    label: "Responses",
                    data: [40, 35, 25],
                    backgroundColor: ["#FF5733", "#337BFF", "#33FF57"]
                }]
            }
        });
    }

    // Checkbox Question Chart
    if (document.getElementById("checkboxChart")) {
        new Chart(document.getElementById("checkboxChart"), {
            type: 'bar',
            data: {
                labels: ["Python", "C#", "JavaScript"],
                datasets: [{
                    label: "Responses",
                    data: [60, 50, 45],
                    backgroundColor: ["#FF5733", "#337BFF", "#33FF57"]
                }]
            }
        });
    }

    // Number Answer (Books Read)
    if (document.getElementById("numberChart")) {
        new Chart(document.getElementById("numberChart"), {
            type: 'line',
            data: {
                labels: ["Min", "Avg", "Max"],
                datasets: [{
                    label: "Books Read",
                    data: [1, 12, 50],
                    backgroundColor: "#33FF57",
                    borderColor: "#33FF57",
                    fill: false
                }]
            }
        });
    }

    // Date Answer
    if (document.getElementById("dateChart")) {
        new Chart(document.getElementById("dateChart"), {
            type: 'line',
            data: {
                labels: ["Jan", "Feb", "Mar", "Apr", "May", "Jun"],
                datasets: [{
                    label: "Responses per Month",
                    data: [10, 15, 30, 20, 25, 10],
                    backgroundColor: "#337BFF",
                    borderColor: "#337BFF",
                    fill: false
                }]
            }
        });
    }
}

// Load real statistics data from the server
async function loadStatistics(questionnaireId) {
    try {
        const response = await fetch(`/Questionnaire/GetStatistics/${questionnaireId}`);
        if (response.ok) {
            const data = await response.json();
            updateStatisticsUI(data);
        } else {
            console.error('Failed to load statistics');
        }
    } catch (error) {
        console.error('Error loading statistics:', error);
    }
}

// Update the UI with real statistics
function updateStatisticsUI(data) {
    // Update total responses
    const totalResponsesElement = document.querySelector('.card:nth-child(1) h3');
    if (totalResponsesElement && data.totalResponses !== undefined) {
        totalResponsesElement.textContent = data.totalResponses;
    }

    // Update completion rate
    const completionRateElement = document.querySelector('.progress-bar');
    if (completionRateElement && data.completionRate !== undefined) {
        completionRateElement.style.width = `${data.completionRate}%`;
        completionRateElement.textContent = `${data.completionRate}%`;
    }

    // Update average time
    const avgTimeElement = document.querySelector('.card:nth-child(3) h3');
    if (avgTimeElement && data.averageTime !== undefined) {
        avgTimeElement.textContent = data.averageTime;
    }

    // Update charts with real data
    // This would be expanded based on the actual data structure returned by the server
} 
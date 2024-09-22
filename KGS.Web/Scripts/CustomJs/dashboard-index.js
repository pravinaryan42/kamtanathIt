$(function () {
    'use strict';
   


    $(document).ready(function () {
        // URL of the controller action that returns chart data
        var url = domain +'dashboard/GetDisrictData';

        // Fetch data from the URL
        $.getJSON(url, function (data) {
            // Process the data for Chart.js
            const labels = data.map(item => item.DistrictName);
            const values = data.map(item => item.Count);
            const backgroundColors = [
                'rgba(255, 99, 132, 0.5)',
                'rgba(54, 162, 235, 0.5)',
                'rgba(255, 206, 86, 0.5)',
                'rgba(75, 192, 192, 0.5)',
                'rgba(153, 102, 255, 0.5)',
                'rgba(255, 159, 64, 0.5)'
            ];
            const borderColors = backgroundColors.map(color => color.replace('0.5', '1'));

            if ($("#pieChart").length) {
                var pieChartCanvas = $("#pieChart").get(0).getContext("2d");
                new Chart(pieChartCanvas, {
                    type: 'pie',
                    data: {
                        datasets: [{
                            data: values,
                            backgroundColor: backgroundColors.slice(0, values.length),
                            borderColor: borderColors.slice(0, values.length)
                        }],
                        labels: labels
                    },
                    options: {
                        responsive: true,
                        animation: {
                            animateScale: true,
                            animateRotate: true
                        }
                    }
                });
            }
        }).fail(function () {
            console.error('Error fetching chart data');
        });

        $.getJSON(domain + 'dashboard/GetUserWiseData', function (data) {
            // Process the data for Chart.js
            const labels = data.map(item => item.DistrictName);
            const values = data.map(item => item.Count);

            // Define background and border colors
            const backgroundColors = [
                'rgba(255, 99, 132, 10)',
                'rgba(54, 162, 235, 10)',
                'rgba(255, 206, 86, 10)',
                'rgba(75, 192, 192, 10)',
                'rgba(153, 102, 255, 10)',
                'rgba(255, 159, 64, 10)'
            ];
            const borderColors = backgroundColors.map(color => color.replace('10', '1'));

            if ($("#barChart").length) {
                var barChartCanvas = $("#barChart").get(0).getContext("2d");
                new Chart(barChartCanvas, {
                    type: 'bar',
                    data: {
                        labels: labels,
                        datasets: [{
                            label: '# of Records',
                            data: values,
                            backgroundColor: backgroundColors.slice(0, values.length),
                            borderColor: borderColors.slice(0, values.length),
                            borderWidth: 1
                        }]
                    },
                    options: {
                        scales: {
                            y: {
                                beginAtZero: true,
                                ticks: {
                                    stepSize: 5, // Set the step size for the Y-axis to 10
                                    callback: function (value, index, values) {
                                        return value; // Display the tick labels as integers
                                    }
                                },
                                // Ensure the max is properly set based on your data
                                // This might help with consistent step sizes
                                max: Math.ceil(Math.max(...values) / 10) * 10
                            }
                        },
                        plugins: {
                            legend: {
                                display: false
                            }
                        },
                        elements: {
                            point: {
                                radius: 0
                            }
                        }
                    }
                });
            }
        }).fail(function () {
            console.error('Error fetching chart data');
        });


        $.getJSON(domain + 'dashboard/GetGPWiseData', function (data) {
            // Process the data for Chart.js
            const labels = data.map(item => item.DistrictName);
            const values = data.map(item => item.Count);

            // Define background and border colors
            const backgroundColors = [
                'rgba(255, 99, 132, 10)',
                'rgba(54, 162, 235, 10)',
                'rgba(255, 206, 86, 10)',
                'rgba(75, 192, 192, 10)',
                'rgba(153, 102, 255, 10)',
                'rgba(255, 159, 64, 10)'
            ];
            const borderColors = backgroundColors.map(color => color.replace('10', '1'));

            if ($("#barChart").length) {
                var barChartCanvas = $("#barChart1").get(0).getContext("2d");
                new Chart(barChartCanvas, {
                    type: 'bar',
                    data: {
                        labels: labels,
                        datasets: [{
                            label: '# of Records',
                            data: values,
                            backgroundColor: backgroundColors.slice(0, values.length),
                            borderColor: borderColors.slice(0, values.length),
                            borderWidth: 1
                        }]
                    },
                    options: {
                        scales: {
                            y: {
                                beginAtZero: true,
                                ticks: {
                                    stepSize: 5, // Set the step size for the Y-axis to 10
                                    callback: function (value, index, values) {
                                        return value; // Display the tick labels as integers
                                    }
                                },
                                // Ensure the max is properly set based on your data
                                // This might help with consistent step sizes
                                max: Math.ceil(Math.max(...values) / 10) * 10
                            }
                        },
                        plugins: {
                            legend: {
                                display: false
                            }
                        },
                        elements: {
                            point: {
                                radius: 0
                            }
                        }
                    }
                });
            }
        }).fail(function () {
            console.error('Error fetching chart data');
        });



    });

});

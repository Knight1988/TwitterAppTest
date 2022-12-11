$(() => {
    const tweetCountChart = document.getElementById('tweetCountChart');
    const connection = new signalR.HubConnectionBuilder().withUrl("/twitterHub").build();
    connection.start().then(function () {
    }).catch(function (err) {
        return console.error(err.toString());
    });

    connection.on("ReceiveAnalytic", function (tweetCount, averageTweetPerMinute) {
        document.getElementById("tweetCount").innerHTML = tweetCount;
        document.getElementById("averageTweetPerMinute").innerHTML = averageTweetPerMinute;
        
        addData(tweetCountChart, {x: '2016-12-25', y: 20})
    });

    function addData(chart,  data) {
        chart.data.datasets.forEach((dataset) => {
            dataset.data.push(data);
        });
        chart.update();
    }

    function removeData(chart) {
        chart.data.labels.pop();
        chart.data.datasets.forEach((dataset) => {
            dataset.data.pop();
        });
        chart.update();
    }

    new Chart(ctx, {
        type: 'line',
        data: {
            datasets: [{
                label: 'Tweet Received',
                data: [{x: '2016-12-25', y: 20}, {x: '2016-12-26', y: 10}]
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });
})


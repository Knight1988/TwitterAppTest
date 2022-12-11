$(() => {
    const tweetCountCanvas = document.getElementById('tweetCountCanvas');
    const connection = new signalR.HubConnectionBuilder().withUrl("/twitterHub").build();

    const tweetCountChart = new Chart(tweetCountCanvas, {
        type: 'line',
        data: {
            labels: [],
            datasets: [{
                label: 'Tweet Received',
                data: [],
                borderWidth: 1
            }]
        }
    });
    
    connection.start().then(function () {
    }).catch(function (err) {
        return console.error(err.toString());
    });

    connection.on("ReceiveAnalytic", function (tweetCount, averageTweetPerMinute) {
        document.getElementById("tweetCount").innerHTML = tweetCount;
        document.getElementById("averageTweetPerMinute").innerHTML = averageTweetPerMinute;
        
        const date = dayjs().format("HH:mm:ss");
        removeData(tweetCountChart);
        addData(tweetCountChart, date, tweetCount);
    });

    function addData(chart, label, data) {
        chart.data.labels.push(label);
        chart.data.datasets.forEach((dataset) => {
            dataset.data.push(data);
        });
        chart.update();
    }

    function removeData(chart) {
        chart.data.datasets.forEach((dataset) => {
            if (dataset.data.length > 10) {
                chart.data.labels.shift();
                dataset.data.shift();
            }
        });
        chart.update();
    }
})


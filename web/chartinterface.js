var option;
var chart;
var ChartType="dynamicLine";
var series;
var DataLoad=[];
function Init() {
    //TODO:
    switch(ChartType)
    {
        case "line":
        {
            option = {
            chart:{
                renderTo:"container"
            },
            title: {
                text: 'Monthly Average Temperature',
                x: -20 //center
            },
            subtitle: {
                text: 'Source: WorldClimate.com',
                x: -20
            },
            xAxis: {
                categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun',
                    'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
            },
            yAxis: {
                title: {
                    text: 'Temperature (°C)'
                },
                plotLines: [{
                    value: 0,
                    width: 1,
                    color: '#808080'
                }]
            },
            tooltip: {
                valueSuffix: '°C'
            },
            legend: {
                layout: 'vertical',
                align: 'right',
                verticalAlign: 'middle',
                borderWidth: 0
            },
            series: [{
                name: 'Tokyo',
                data: [7.0, 6.9, 9.5, 14.5, 18.2, 21.5, 25.2, 26.5, 23.3, 18.3, 13.9, 9.6]
            }]
        };
        break;
        }
        case "dynamicLine":
        {
            option={
            chart: {
                renderTo:"container",
                type: 'spline',
                animation: Highcharts.svg, // don't animate in old IE
                marginRight: 10,
                events: {
                    load: function() {
    
                        // set up the updating of the chart each second
                        series = this.series[0];
                        // setInterval(function() {
                        //     var x = (new Date()).getTime(), // current time
                        //         y = Math.random();
                        //     series.addPoint([x, y], true, true);
                        // }, 1000);
                    }
                }
            },
            title: {
                text: 'Live random data'
            },
            xAxis: {
                type: 'datetime',
                tickPixelInterval: 150
            },
            yAxis: {
                title: {
                    text: 'Value'
                },
                plotLines: [{
                    value: 0,
                    width: 1,
                    color: '#808080'
                }]
            },
            tooltip: {
                formatter: function() {
                        return '<b>'+ this.series.name +'</b><br/>'+
                        Highcharts.dateFormat('%Y-%m-%d %H:%M:%S', this.x) +'<br/>'+
                        Highcharts.numberFormat(this.y, 2);
                }
            },
            legend: {
                enabled: false
            },
            exporting: {
                enabled: false
            },
            series: [{
                name: 'Random data',
                data: DataLoad
            }]
        };
        break;
        }
    }
    
}
function LoadChartType(charttype) {
    //TODO:
    ChartType=charttype;
}
function LoadData(datatype, column, data) {
    var time=eval(column);
    var data=eval(data);
    switch(datatype)
    {
        case "runtime":
        {
            DataLoad.push([time,data]);
            break;
        }
    }
}
function LoadDataArray(datatype, column, data) {
    var time = eval(column);
    var data = eval(data);
    var tmp=[];
    for(var i=0;i<time.length;++i)
    {
        tmp.push([time[i],data[i]]);
    }
    //TODO:loaddata run before Start
    //datatype:enum of "RunTime","PointToPoint","Smooth","DayGray","VariationCoefficient"
    //eg
    //datatype="runtime"
    //time="["2013-12-25 21:00:00","2013-12-25 22:00:00","2013-12-25 22:15:00"]"
    //data="[20,40,-100]"
    switch(datatype){
        case "runtime": {
            if (option["series"][0]) {
                for(var i=0;i<tmp.length;++I)
                    option["series"][0].push(tmp[i]);
            }
            else {
                option["series"][0] = tmp;
            }
        }
    }
}

function Start() {
    //TODO:print the chart
    Init();
    chart = new Highcharts.Chart(option);
}
function AddData(datatype, column, data) {
    //TODO: runtime add single data (runtime)
    //eg
    //datatype="runtime"
    //time="2013-12-25 21:00:00"
    //data=20
    switch (datatype) {
        case "runtime": {
            var x=eval(column);
            var y=eval(data);
            chart.series[0].addPoint([x,y],true,true);
            // series[0].addPoint([x,y],true,true);
        }
    }
}
function AddDataArray(datatype, column, data) {
    //TODO: runtime add data array (predict)
    //eg
    //datatype="runtime"
    //time="["2013-12-25 21:00:00","2013-12-25 22:00:00","2013-12-25 22:15:00"]"
    //data="[20,40,-100]"

}
function DeleteDataSeries(datatype) {
    //TODO: delete the whole series
    //warning: can reload the series
}
function LoadWrap()
{
    for(var i=0;i<20;++i)
    {
        var x = (new Date()).getTime()+i, // current time
            y = Math.random();
        LoadData("runtime",x,y);
    }
    
}
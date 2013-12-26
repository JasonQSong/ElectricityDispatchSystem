var option;
var chart;
function Init() {
    //TODO:
    option = { chart: { renderto: "container" } };
}
function LoadChartType(charttype) {
    //TODO:
}
function LoadData(datatype, column, data) {

}
function LoadDataArray(datatype, column, data) {
    time = eval(time);
    data = eval(data);
    //TODO:loaddata run before Start
    //datatype:enum of "RunTime","PointToPoint","Smooth","DayGray","VariationCoefficient"
    //eg
    //datatype="runtime"
    //time="["2013-12-25 21:00:00","2013-12-25 22:00:00","2013-12-25 22:15:00"]"
    //data="[20,40,-100]"
    switch(datatype){
        case "runtime": {
            if (option["series"][0]) {
                option["series"][0].push();
            }
            else {
                option["series"][0] = data;
            }
        }
    }
}

function Start() {
    //TODO:print the chart
    Init();
    chart = new Highchart();
}
function AddData(datatype, column, data) {
    //TODO: runtime add single data (runtime)
    //eg
    //datatype="runtime"
    //time="2013-12-25 21:00:00"
    //data=20
    switch (datatype) {
        case "runtime": {
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
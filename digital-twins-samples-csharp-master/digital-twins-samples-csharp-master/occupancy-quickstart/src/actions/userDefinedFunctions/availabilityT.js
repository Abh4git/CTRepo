// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

var levelType = "Level";
var positionType = "ValvePosition";
var spaceAvailFresh = "AvailableAndFresh";
var levelThresholdHigh = 1000.0;
var levelThresholdMedium = 500.0;
var levelThresholdHigh = 200.0;
var spaceLevelLow = "Level Low";
var spaceLevelHigh = "Level High";
var spaceLevelMedium = "Level Medium";
// Add your sensor type here

function process(telemetry, executionContext) {

    try {
        // Log SensorId and Message
        log(`Sensor ID: ${telemetry.SensorId}. `);
        log(`Sensor value: ${JSON.stringify(telemetry.Message)}.`);

        // Get sensor metadata
        var sensor = getSensorMetadata(telemetry.SensorId);

        // Retrieve the sensor reading
        var parseReading = JSON.parse(telemetry.Message);

        // Set the sensor reading as the current value for the sensor.
        setSensorValue(telemetry.SensorId, sensor.DataType, parseReading.SensorValue);

        // Get parent space
        var parentSpace = sensor.Space();

        // Get children sensors from the same space
        //var otherSensors = parentSpace.ChildSensors();

        // Retrieve carbonDioxide, and motion sensors
        //var positionSensor = otherSensors.find(function(element) {
          //  return element.DataType === positionType;
        //});

        //var levelSensor = otherSensors.find(function(element) {
          //  log(`levelSensor found `);
            //return element.DataType === levelType;
        //});
        log(`Reached here-Abh`);
        // Add your sensor variable here

        // get latest values for above sensors
        var levelValue = getFloatValue(parseReading.SensorValue);
        var presence = !!levelValue && levelValue.toLowerCase() === "true";
        var positionValue = getFloatValue(positionSensor.Value().Value);

        // Add your sensor latest value here
        
        // Return if no motion or carbonDioxide found return
        // Modify this line to monitor your sensor value
        if(positionValue === null || levelValue === null) {
            sendNotification(telemetry.SensorId, "Sensor", "Error: Position or Level are null, returning");
            return;
        }

        // Modify these lines as per your sensor
        var levelLowerThan = "Room is available and air is fresh";
        var noAvailableOrFresh = "Room is not available or air quality is poor";

        // Modify this code block for your sensor
        // If carbonDioxide less than threshold and no presence in the room => log, notify and set parent space computed value
        if(parseFloat(levelValue) > parseFloat(levelThresholdHigh)  ) {

            log(`${spaceLevelHigh}. Level : ${levelValue}. `);
            setSpaceValue(parentSpace.Id, spaceAvailFresh, "High");
            parentSpace.Notify(JSON.stringify(spaceLevelHigh));
        }
        else 
        {
            if (parseFloat(levelValue) > parseFloat( levelThresholdMedium))
            {
                log(`${spaceLevelMedium}. Level : ${levelValue}.`);
                setSpaceValue(parentSpace.Id, spaceAvailFresh, "Medium");
            }
            else {
                log(`${spaceLevelLow}. Level: ${levelValue}. `);
                setSpaceValue(parentSpace.Id, spaceAvailFresh, "Low");
                // Set up custom notification for poor air quality
                parentSpace.Notify(JSON.stringify(spaceLevelLow));
        }
    }
    }
    catch (error)
    {
        log(`An error has occurred processing the UDF Error: ${error.name} Message ${error.message}.`);
    }
}

function getFloatValue(str) {
  if(!str) {
      return null;
  }

  return parseFloat(str);
}

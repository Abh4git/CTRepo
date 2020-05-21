// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

var levelType = "Level";
var valvePosType = "ValvePosition";
var levelAlarm = "LevelMedium";
var levelThreshold = 500.0;
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
        var otherSensors = parentSpace.ChildSensors();
        if (otherSensors!=null)
        {
            log(`Child Sensors Found. `);
        }
        // Retrieve carbonDioxide, and motion sensors
        var levelSensor = otherSensors.find(function(element) {
            return element.DataType === levelType;
        });

        if (levelSensor !=null)
        {
            log(`Level Sensor Found. `);
        }
        var valvePosSensor = otherSensors.find(function(element) {
            return element.DataType === valvePosType;
        });
        
        // Add your sensor variable here

        // get latest values for above sensors
        var positionValue = valvePosSensor.Value().Value;
        //var presence = !!motionValue && motionValue.toLowerCase() === "true";
        //var levelValue = getFloatValue(levelSensor.Value().Value);
        var levelValue = getFloatValue(parseReading.SensorValue);
        log(`Got Float Value.`);

        // Add your sensor latest value here
        
        // Return if no motion or carbonDioxide found return
        // Modify this line to monitor your sensor value
        if(levelValue == null ) {
            sendNotification(telemetry.SensorId, "Sensor", "Error: Level null");
            //return;
            log(`notification as null.`);

        }

        // Modify these lines as per your sensor
        var levelLow = "Low";
        var levelMedium = "Medium";

        log(`About to check Level Thereshold`);


        // Modify this code block for your sensor
        // If Level less than threshold  => log, notify and set parent space computed value
        if(parseFloat(levelValue) < parseFloat(levelThreshold) ) {
            log(`${levelLow}. Level : ${levelValue}.`);
            setSpaceValue(parentSpace.Id, levelAlarm, levelLow);
             // Set up custom notification for low level 
             parentSpace.Notify(JSON.stringify(levelAlarm));
        }
        else {
            log(`${levelMedium}. Level: ${levelValue}`);
            setSpaceValue(parentSpace.Id, levelAlarm, levelMedium);

           
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

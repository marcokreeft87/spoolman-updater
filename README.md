# Spoolman Updater API

## Overview

The Spoolman Updater API provides endpoints to manage spool updates, including tracking filament usage and material details. This API is designed to work with Home Assistant and other automation systems.

To facilitate API development and testing, the Spoolman Updater API utilizes Swagger for interactive API documentation. You can access the Swagger UI at http://<your-server>:8088/swagger, which allows you to explore and test the available endpoints.

## Base URL

```
http://<your-server>:8088
```

## Endpoints

### 1. Update Spool

#### **Endpoint:**

```
POST /spool
```

#### **Description:**

Updates the spool details based on filament usage.

#### **Request Body:**

```json
{
  "name": "Bambu PLA Basic",
  "material": "PLA",
  "tagUid": "0000000000000000",
  "usedWeight": 10,
  "color": "#FFFFFFFF",
  "activeTrayId": "tray1"
}
```

#### **Response:**

- **200 OK**: Successfully updated spool.
- **400 Bad Request**: Missing required fields.

---

## Environment Variables

The API requires the following environment variables to be set:

```
APPLICATION__HOMEASSISTANT__URL=http://homeassistant.local
APPLICATION__HOMEASSISTANT__TOKEN=your-token
APPLICATION__HOMEASSISTANT__AMSENTITIES=sensor.x1c_ams_1
APPLICATION__HOMEASSISTANT__AMSEXTERNALSPOOL=sensor.x1c_external_spool
APPLICATION__SPOOLMAN__URL=http://spoolman.local



{
  "Application": {
    "HomeAssistant": {
      "Url": "http://192.168.2.4:8123",
      "Token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiI2ZGYzNGE2MmIyMTI0YmI3OTI0OTVjM2VmMTFlYWI5ZiIsImlhdCI6MTc0MTc5OTQwMCwiZXhwIjoyMDU3MTU5NDAwfQ.MG_gLT4NBkMrvCNavgk1fB3rXN9mu9RptGOsgzyBIT4",
      "AMSEntities": [
        "X1C_00M09C422100420_AMS_1"
      ],
      "ExternalSpoolEntity": "sensor.x1x_externalspool_external_spool"
    },
    "Spoolman": {
      "Url": "http://192.168.2.186:7912",
      "PrinterId": "00M09C422100420"
    }
  }
}

```

## Running with Docker

### **Build the Docker image**

```
docker build -t spoolman-updater .
```

### **Run the container**

```
docker run -d -p 8088:8080 \
  -e APPLICATION__HOMEASSISTANT__URL=http://homeassistant.local \
  -e APPLICATION__HOMEASSISTANT__TOKEN=your-token \
  -e APPLICATION__SPOOLMAN__URL=http://spoolman.local \
  -e APPLICATION__HOMEASSISTANT__AMSENTITIES=sensor.x1c_ams_1 \
  -e APPLICATION__HOMEASSISTANT__AMSEXTERNALSPOOL=sensor.x1c_external_spool \
  --name spoolman-updater spoolman-updater
```

## Logging

The API logs requests and responses using the default ASP.NET logging system. You can configure logging levels in `appsettings.json`:

```json
"Logging": {
  "LogLevel": {
    "Default": "Information",
    "Microsoft.AspNetCore": "Warning"
  }
}
```

---

## Using with Home Assistant
The Spoolman Updater API can be integrated into Home Assistant automations to track filament usage automatically.

### **1. Define a REST Command in `configuration.yaml`**
Add the following to your `configuration.yaml` to create a REST command that updates the spool:

```yaml
rest_command:
  update_spool:
    url: "http://<your-server>:8088/Spools/spool"
    method: POST
    headers:
      Content-Type: "application/json"
    payload: >
      {
        "name": "{{ name }}",
        "material": "{{ material }}",
        "tag_uid": "{{ tag_uid }}",
        "used_weight": {{ used_weight }},
        "color": "{{ color }}",
        "active_tray_id": "{{ filament_active_tray_id }}"
      }
```

### **2. Create an Automation**
The following automation updates the spool when a print finishes or when the AMS tray switches:

```yaml
alias: Bambulab - Update Spool When Print Finishes or Tray Switches
description: ""
triggers:
  - trigger: state
    entity_id:
      - sensor.x1c_active_tray_index
conditions:
  - condition: template
    value_template: "{{ trigger.from_state.state not in ['unknown', 'unavailable'] }}"
  - condition: template
    value_template: "{{ trigger.from_state.state | int > 0 }}"
actions:
  - variables:
      tray_number: >-
        {{ trigger.from_state.state if trigger.entity_id ==
        'sensor.x1c_active_tray_index' else
        states('sensor.x1c_active_tray_index') }}
      tray_sensor: sensor.x1c_00m09c422100420_ams_1_tray_{{ tray_number }}
      tray_weight: >-
        {{ states('sensor.bambulab_filament_usage_meter') | float(0) | round(2)
        }}
      tag_uid: "{{ state_attr(tray_sensor, 'tag_uid') }}"
      material: "{{ state_attr(tray_sensor, 'type') }}"
      name: "{{ state_attr(tray_sensor, 'name') }}"
      color: "{{ state_attr(tray_sensor, 'color') }}"
  - data:
      filament_name: "{{ name }}"
      filament_material: "{{ material }}"
      filament_tag_uid: "{{ tag_uid }}"
      filament_used_weight: "{{ tray_weight }}"
      filament_color: "{{ color }}"
      filament_active_tray_id: "{{ tray_sensor | replace('sensor.', '') }}"
    action: rest_command.update_spool
  - action: utility_meter.calibrate
    data:
      value: "0"
    target:
      entity_id: sensor.bambulab_filament_usage_meter

```

This automation ensures that the filament usage is automatically updated in Spoolman when a print is completed or the AMS tray is changed.

---

### Setting the active tray in the UI when switching spools
When you switch your spool in the AMS, you will need to tell spoolman which tray the new spool is in. You can do this in the UI of Spoolman updater.
Just go to the base of the URL of the API. So for example if your API url is http://192.168.2.186:8088/spools you go to http://192.168.2.186:8088/

![alt text](image.png)

Here you can set which spool is in which tray. 

## Contributing

Pull requests are welcome! Please follow the standard GitHub workflow:

1. Fork the repository
2. Create a feature branch
3. Submit a pull request

## License

MIT License. See `LICENSE` file for details.


import { HomeAssistantConfiguration } from "./homeassistant-configuration";
import { SpoolmanConfiguration } from "./spoolman-configuration";

export interface Configuration {
  spoolman: SpoolmanConfiguration;
  home_assistant: HomeAssistantConfiguration;
}
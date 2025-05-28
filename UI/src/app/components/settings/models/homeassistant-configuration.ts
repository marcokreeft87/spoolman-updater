export interface HomeAssistantConfiguration {
  url: string;
  token: string;
  external_spool_entity: string;
  ams_entities: string[];
}
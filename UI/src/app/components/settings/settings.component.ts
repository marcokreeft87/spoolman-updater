import { Component } from "@angular/core";
import { FormBuilder, FormGroup, ReactiveFormsModule } from "@angular/forms";
import { MatCardModule } from "@angular/material/card";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatIcon } from "@angular/material/icon";
import { Configuration } from "./models/configuration";
import { SettingssService } from "./services/settings.service";

@Component({
  selector: 'settings-component',
  standalone: true,
  imports: [
    MatCardModule,
    MatFormFieldModule,
    MatIcon,
    ReactiveFormsModule
  ],
  providers: [SettingssService],
  templateUrl: './settings.component.html'
})
export class SettingsComponent {
save() {
throw new Error('Method not implemented.');
}

    form!: FormGroup;
    configuration!: Configuration;

    constructor(
        private service: SettingssService,
        private fb: FormBuilder) {
        this.service.get().subscribe(configuration => this.createForm(configuration));
    }

    createForm(configuration: Configuration): void {
        this.form = this.fb.group({
            spoolman: this.fb.group({
                url: [configuration.spoolman.url],
            }),
            home_assistant: this.fb.group({
                ams_entities: [configuration.home_assistant.ams_entities || []],
                external_spool_entity: [configuration.home_assistant.external_spool_entity || '']
            })
        });
    }
}
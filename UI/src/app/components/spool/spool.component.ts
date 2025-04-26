import { Component, Host, HostBinding, ViewEncapsulation } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { HttpClientModule } from '@angular/common/http';
import { MatSelectChange, MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { CommonModule } from '@angular/common';
import { SpoolsService } from '../../shared/service/spoolman.service';
import { TrayService } from '../../shared/service/tray.service';
import { AMSEntity, Tray } from '../../shared/models/tray';
import { Spool } from '../../shared/models/spool';
import { TrayComponent } from '../../shared/components/tray/tray.component';

@Component({
  selector: 'spool-component',
  standalone: true,
  imports: [
    MatCardModule,
    TrayComponent
  ],
  templateUrl: './spool.component.html',
  styleUrl: './spool.component.scss',
  providers: [SpoolsService, TrayService],
  encapsulation: ViewEncapsulation.None,
})
export class SpoolComponent {
  @HostBinding('class') className = 'spool-component';

  spools: Spool[] = [];
  amsEntities: AMSEntity[] = [];
  externalSpoolEntity: Tray = {} as Tray;

  constructor(
    private spoolService: SpoolsService,
    private trayService: TrayService
  ) {
    this.spoolService.getSpools().subscribe((spools: Spool[]) => {
      this.spools = spools;
    });

    this.trayService.getTrays().subscribe(({ ams_entities, external_spool_entity }) => {
      this.amsEntities = ams_entities;
      this.externalSpoolEntity = external_spool_entity;
    });
  }

  
}

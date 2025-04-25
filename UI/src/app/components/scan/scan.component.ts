import { Component } from '@angular/core';
import { SpoolsService } from '../../shared/service/spoolman.service';
import { TrayService } from '../../shared/service/tray.service';
import { Spool } from '../../shared/models/spool';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { AMSEntity, Tray } from '../../shared/models/tray';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-scan',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatButtonModule],
  templateUrl: './scan.component.html',
  styleUrl: './scan.component.scss',
  providers: [SpoolsService, TrayService],
})
export class ScanComponent {
  spool: Spool | undefined;
  amsEntities: AMSEntity[] = [];
  externalSpoolEntity: Tray = {} as Tray;

  constructor(
    private route: ActivatedRoute,
    private spoolService: SpoolsService, 
    trayService: TrayService
  ) {

    this.route.queryParamMap.subscribe(params => {
      const barcode = params.get('barcode') ?? '';
      
      spoolService.getByBarcode(barcode).subscribe((spools: Spool[]) => {
        this.spool = spools[0];
      });
  
      trayService
        .getTrays()
        .subscribe(({ ams_entities, external_spool_entity }) => {
          this.amsEntities = ams_entities;
          this.externalSpoolEntity = external_spool_entity;
        });
    });

    
  }

  updateTray(tray: Tray | undefined): void {
    if (!tray || !this.spool) 
      return;    

    this.spoolService
      .updateTray({
        spool_id: this.spool.id.toString(),
        active_tray_id: tray.id,
      })
      .subscribe();
  }
}

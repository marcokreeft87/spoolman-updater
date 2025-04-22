import { Component } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { SpoolsService } from './service/spoolman.service';
import { HttpClientModule } from '@angular/common/http';
import { TrayService } from './service/tray.service';
import { MatSelectChange, MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { Tray } from './models/tray';
import { Spool } from './models/spool';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    HttpClientModule,
    CommonModule,
    MatToolbarModule,
    MatButtonModule,
    MatCardModule,
    MatSelectModule,
    MatFormFieldModule,
  ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
  providers: [SpoolsService, TrayService],
})
export class AppComponent {
  title = 'spoolman-updater';

  spools: Spool[] = [];
  trays: Tray[] = [];

  constructor(
    private spoolService: SpoolsService,
    private trayService: TrayService
  ) {
    this.spoolService.getSpools().subscribe((spools: Spool[]) => {
      this.spools = spools;
    });

    this.trayService.getTrays().subscribe((trays: Tray[]) => {
      this.trays = trays;
    });
  }

  displaySpoolName(spool: Spool): string {
    return spool
      ? `${spool.filament.vendor.name} ${spool.filament.material} ${spool.filament.name} (#${spool.filament.color_hex}) ${spool.remaining_weight}g`
      : '';
  }

  getCurrentSpool(tray: Tray): Spool {
    const currentSpool = this.spools.filter((spool) =>
      spool.extra['active_tray']?.includes(tray.id)
    )[0];

    return currentSpool;
  }

  onSpoolChange(selectChange: MatSelectChange, tray: Tray): void {
    const selectedSpoolId = selectChange.value;

    this.setSpoolToTray(selectedSpoolId, tray);
  }

  setSpoolToTray(selectedSpoolId: string, tray: Tray): void {
    tray.selectedSpool = selectedSpoolId;
  }

  updateTray(tray: Tray): void {
    this.spoolService
      .updateTray({
        spool_id: tray.selectedSpool,
        active_tray_id: tray.id,
      })
      .subscribe();
  }
}

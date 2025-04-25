import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectChange, MatSelectModule } from '@angular/material/select';
import { Tray } from '../../models/tray';
import { Spool } from '../../models/spool';
import { SpoolsService } from '../../service/spoolman.service';

@Component({
  selector: 'app-tray',
  standalone: true,
  imports: [
      MatButtonModule,
      MatCardModule,
      MatSelectModule,
      MatFormFieldModule,
      CommonModule
  ],
  templateUrl: './tray.component.html',
  styleUrl: './tray.component.scss'
})
export class TrayComponent {
  @Input() tray: Tray | undefined;
  @Input() spools: Spool[] = [];
  @Input() name: string = '';

  constructor(
      private spoolService: SpoolsService
    ) { }

  displaySpoolName(spool: Spool): string {
    return spool
      ? `${spool.filament.vendor.name} ${spool.filament.material} ${spool.filament.name} - ${spool.remaining_weight}g`
      : '';
  }

  getCurrentSpool(tray: Tray | undefined): Spool {
    if (!tray) {
      return {} as Spool;
    }

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

  updateTray(tray: Tray | undefined): void {
    if (!tray) 
      return;    

    this.spoolService
      .updateTray({
        spool_id: tray.selectedSpool,
        active_tray_id: tray.id,
      })
      .subscribe();
  }
}

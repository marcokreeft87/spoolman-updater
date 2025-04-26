import { Component, Host, HostBinding, ViewEncapsulation } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { HttpClientModule } from '@angular/common/http';
import { MatSelectChange, MatSelectModule } from '@angular/material/select';
import { MatFormFieldModule } from '@angular/material/form-field';
import { AsyncPipe, CommonModule } from '@angular/common';
import { SpoolsService } from '../../shared/service/spoolman.service';
import { TrayService } from '../../shared/service/tray.service';
import { AMSEntity, Tray } from '../../shared/models/tray';
import { Spool } from '../../shared/models/spool';
import { TrayComponent } from '../../shared/components/tray/tray.component';
import { BehaviorSubject, forkJoin, Subject } from 'rxjs';

@Component({
  selector: 'spool-component',
  standalone: true,
  imports: [
    MatCardModule,
    TrayComponent,
    AsyncPipe
  ],
  templateUrl: './spool.component.html',
  styleUrl: './spool.component.scss',
  providers: [SpoolsService, TrayService],
  encapsulation: ViewEncapsulation.None,
})
export class SpoolComponent {
  @HostBinding('class') className = 'spool-component';

  spools: Spool[] = [];


  externalSpoolEntity$ = new BehaviorSubject<Tray>({} as Tray);  
  amsEntities$ = new BehaviorSubject<AMSEntity[]>([]);

  amsEntities = this.amsEntities$.asObservable();
  externalSpoolEntity = this.externalSpoolEntity$.asObservable();

  constructor(
    private spoolService: SpoolsService,
    private trayService: TrayService
  ) {
    
    forkJoin({
      spools: this.spoolService.getSpools(),
      trays: this.trayService.getTrays()
    }).subscribe(({ spools, trays }) => {
      this.spools = spools;
      this.amsEntities$.next(trays.ams_entities);
      this.externalSpoolEntity$.next(trays.external_spool_entity);
    });
  }  
}

import { AsyncPipe } from '@angular/common';
import { Component, HostBinding, ViewEncapsulation } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { BehaviorSubject, forkJoin } from 'rxjs';
import { TrayComponent } from '../../shared/components/tray/tray.component';
import { Spool } from '../../shared/models/spool';
import { AMSEntity, Tray } from '../../shared/models/tray';
import { SpoolsService } from '../../shared/service/spoolman.service';
import { TrayService } from '../../shared/service/tray.service';

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

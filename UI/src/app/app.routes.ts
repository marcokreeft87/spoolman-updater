import { Routes } from '@angular/router';
import { SpoolComponent } from './components/spool/spool.component';
import { ScanComponent } from './components/scan/scan.component';

export const routes: Routes = [
    { path: '', component: SpoolComponent },
    { path: 'scan', component: ScanComponent },
];

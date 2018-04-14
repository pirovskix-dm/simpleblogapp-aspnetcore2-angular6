import { GlobalEventsService } from './../../services/globalevents.service';
import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';

@Component({
    selector: 'nav-menu',
    templateUrl: './navmenu.component.html',
    styleUrls: ['./navmenu.component.css']
})
export class NavMenuComponent {
    showAdminNav: boolean = false;
    constructor(
        private userService: UserService,
        private router: Router,
        private globalEventsService: GlobalEventsService
    ) { 
        this.globalEventsService.showAdminNavBar.subscribe((mode: boolean) => {
            this.showAdminNav = mode;
        });
    }
    onLogOut(){
        this.userService.userLogOff()
            .subscribe(_ => {
                this.globalEventsService.showAdminNavBar.emit(false);
                this.router.navigate(['']);
            });
    }
}

import { Injectable, EventEmitter } from '@angular/core';

@Injectable()
export class GlobalEventsService {
    public showAdminNavBar: EventEmitter<boolean> = new EventEmitter();
}
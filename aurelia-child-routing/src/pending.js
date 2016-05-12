import {LogManager} from 'aurelia-framework';

const logger = LogManager.getLogger('PP');

export class PendingPayments {
    constructor() {
        this.title = "Pending payments";
    }

    canActivate(params, routeConfig, navigationInstruction) {                    // 2
        logger.debug("canActivate", params, routeConfig, navigationInstruction);
    }

    activate(params, routeConfig, navigationInstruction) {                       // 3
        logger.debug("activate", params, routeConfig, navigationInstruction);
        // this.eventAggregator.subscribe(this.payrollHub.pingEvent, e => {
        //     logger.debug("Received a ping event", e);
        //     this.pingMessage = "Ping message: " + e;
        // });
    }

    // Component/behavior lifecycle hooks. See http://aurelia.io/docs.html#extending-html
    created(view) {                                                              // 4
        logger.debug('created', view);
    }

    bind(bindingContext) {                                                       // 5
        logger.debug('bind', bindingContext);
    }

    attached() {                                                                 // 6
        logger.debug("attached");
    }

    canDeactivate() {                                                            // 7
        logger.debug('canDeactivate');
    }

    deactivate() {                                                               // 8
        logger.debug('deactivate');
    }

}

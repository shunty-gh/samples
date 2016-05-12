import {LogManager} from 'aurelia-framework';

const logger = LogManager.getLogger('Parent');

export class Parent {
    constructor() {
        this.title = 'Hello payrolls';
        this.vm = this;
    }

    configureRouter(config, router) {
        config.map([
            { route: ['', 'summary'], name: 'summary',       moduleId: 'summary',       nav: true, title: 'Summary' },
            { route: 'pending',         name: 'pending',         moduleId: 'pending',         nav: true, title: 'Pending payments', settings: { parent: this } },
            { route: 'default',  name: 'default',  moduleId: 'default',  nav: true, title: 'Default payments' }
        ]);

        this.router = router;
    }

    activate(params) {
        this.loadPayrolls();

        this.pId = parseInt(params.parentId || 0, 10);
        logger.debug("Parent activate", this);

        switch (this.pId) {
            case 1:
                this.payrolls.push({ id: 101, name: "Fred"});
                break;
            case 2:
                this.payrolls.push({ id: 201, name: "Jim"});
                this.payrolls.push({ id: 301, name: "Charlie"});
                break;
            default:
                break;
        }
    }

    loadPayrolls() {
        this.payrolls = [
            { id: 1, 'name': 'P1' },
            { id: 2, 'name': 'P2' },
            { id: 3, 'name': 'P3' },
            { id: 4, 'name': 'P4' }
        ];
    }
}

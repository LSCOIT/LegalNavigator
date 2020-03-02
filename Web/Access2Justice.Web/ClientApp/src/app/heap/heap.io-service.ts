import { Injectable } from '@angular/core';
import { IHeapIoOptions } from '../heap/heap.io-interface';

declare global {
    interface Window {
        heap: any;
        debug: boolean;
    }
}

@Injectable({
    providedIn: 'root'
})
export class HeapIoService {
    /** Heap IO API object */
    private service: any;

    constructor() {
        window.debug = true;
        const heap = window.heap || [];
        heap.load = function(e, t) {
            (window.heap.appid = e), (window.heap.config = t = t || {});
            let r = t.forceSSL || 'https:' === document.location.protocol,
                a = document.createElement('script');
            (a.type = 'text/javascript'),
                (a.async = !0),
                (a.src = (r ? 'https:' : 'http:') + '//cdn.heapanalytics.com/js/heap-' + e + '.js');
            let n = document.getElementsByTagName('script')[0];
            n.parentNode.insertBefore(a, n);
            for (
                let o = function(e) {
                        return function() {
                            heap.push([e].concat(Array.prototype.slice.call(arguments, 0)));
                        };
                    },
                    p = [
                        'addEventProperties',
                        'addUserProperties',
                        'clearEventProperties',
                        'identify',
                        'removeEventProperty',
                        'setEventProperties',
                        'track',
                        'unsetEventProperty'
                    ],
                    c = 0;
                c < p.length;
                c++
            )
                heap[p[c]] = o(p[c]);
        };
        window.heap = heap;
        this.service = heap;
    }

    /**
     * Initialise heap io with the given parameters
     * @param options
     */
    public load(options: IHeapIoOptions) {
        if (!this.service) { throw new Error('Heap IO is not initialised'); }
        const heap_ops = {
            forceSSL: options.force_ssl,
            secureCookie: options.secure_cookie,
            disableTextCapture: options.disable_text_capture,
            cookiePath: options.cookie_path
        }
        this.service.load(options.app_id, heap_ops);
    }

    /**
     * Set the identity of the Heap IO session
     * @param id Identity of the active session user
     */
    public identify(id: string) {
        if (!this.service) { throw new Error('Heap IO is not initialised'); }
        this.service.identify(id);
    }

    /**
     * Add properties associated with the identity given for the session
     * @param properties Map of properties. e.g. `{ prop1: prop1_value, prop2: prop2_value, ... }`
     */
    public addUserProperties(properties: { [name:string]: any }) {
        if (!this.service) { throw new Error('Heap IO is not initialised'); }
        this.service.addUserProperties(properties);
    }

    /**
     * Post new tracking event
     * @param event_name Name of the event
     * @param properties Map of event properties. e.g. `{ prop1: prop1_value, prop2: prop2_value, ... }`
     */
    public track(event_name: string, properties: { [name:string]: any }) {
        if (!this.service) { throw new Error('Heap IO is not initialised'); }
        
        this.service.track(event_name, properties);
    }

}
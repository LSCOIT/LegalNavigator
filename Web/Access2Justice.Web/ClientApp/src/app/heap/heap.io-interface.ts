export interface IHeapIoOptions {
    /** Heap IO application identifier */
    app_id: string | number;
    /** Whether to force Heap IO connection to use SSL */
    force_ssl: boolean;
    /** Whether Heap IO cookies should be secure */
    secure_cookie: boolean;
    /** Whether Heap IO should not capture element content text */
    disable_text_capture: boolean;
    /** Limit Heap IO API cookies to given path on the domain */
    cookie_path: string;
}
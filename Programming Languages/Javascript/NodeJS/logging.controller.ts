import { Environment } from "../helpers/env-enum";
import * as winston from "winston";

//Used like this: LoggingController.logger.debug(`this is a debug log`);
export class LoggingController {
  private static _instance: winston.LoggerInstance;

  static get logger(): winston.LoggerInstance {
    if (this._instance === null || this._instance === undefined) {
      this._instance = new (winston.Logger)({
        transports: [
          new winston.transports.File({
            colorize: true,
            filename: "./Application.log",
            handleExceptions: true,
            json: true,
            level: "silly",
            maxFiles: 5,
            maxsize: 5242880, // 5MB
          }),
          new (winston.transports.Console)({
            colorize: true,
            json: true,
            level: "silly",
            name: "Console",
          }),
        ],
      });
    }
    // Prevent all logging or unit tests
    let env: string = process.env.NODE_ENV ? process.env.NODE_ENV.trim() : Environment.development;
    if (env === Environment.test) {
      this._instance = new winston.Logger();
    }
    return this._instance;
  }
}

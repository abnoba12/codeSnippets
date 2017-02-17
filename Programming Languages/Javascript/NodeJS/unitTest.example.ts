/* tslint:disable:no-any */
import {UserController} from "../controllers/user.controller";
import APIError from "../helpers/api-error";
import { User } from "../models/user.model";
import { UserPost } from "../models/userpost.model";
import { MongooseRepository } from "../repository/mongoose.repository";
import { IUserDocument } from "../repository/user.model.mongoose";
import { TestObjects } from "./test.objects";
import * as chai from "chai";
import * as express from "express";
import * as httpStatus from "http-status";
import * as mongoose from "mongoose";
import * as requestPromise from "request-promise";
import * as sinon from "sinon";

require("mongoose").Promise = global.Promise;

let expect: any = chai.expect;
let _userCont: UserController;

describe("user.controller", () => {
    beforeEach(done => {
        _userCont = new UserController();
        done();
    });

    describe("getUserById", () => {
        it("Success", sinon.test(function (done: any): any {
            // Assemble
            let userDoc: IUserDocument = TestObjects.mongooseUser;
            this.stub(mongoose.Query.prototype, "exec")
                .returns(Promise.resolve<User>(userDoc));

            // Act
            _userCont.getUserById(userDoc.id)
                .then(res => {
                    // Assert
                    expect(res.username).to.be.eql(userDoc.username);
                    expect(res.mobile_number).to.be.eql(userDoc.mobile_number);

                    done();
                });
        }));

        it("UserNotFound_Throws", sinon.test(function (done: any): any {
            // Assemble
            this.stub(mongoose.Query.prototype, "exec")
                .returns(Promise.resolve<User>(null));

            // Act
            _userCont.getUserById(new mongoose.Types.ObjectId().toString())
                .catch(res => {
                    // Assert
                    expect(res).to.be.an.instanceof(APIError);
                    expect(res.status).to.be.eql(404);
                    expect(res.message).to.be.eql("No such user exists!");

                    done();
                });
        }));

        it("nullId_Throws", sinon.test(function (done: any): any {
            // Assemble
            let userDoc: IUserDocument = TestObjects.mongooseUser;
            this.stub(mongoose.Query.prototype, "exec")
                .returns(Promise.resolve<User>(userDoc));

            // Act
            _userCont.getUserById(null)
                .catch(res => {
                    // Assert
                    expect(res).to.be.an.instanceof(APIError);
                    expect(res.message).to.be.eql("ID is required");

                    done();
                });
        }));

        it("repoCallFails_Throws", sinon.test(function (done: any): any {
            // Assemble
            let user: User = TestObjects.user;
            this.stub(mongoose.Query.prototype, "exec")
                .returns(Promise.reject("Call to DB failed"));

            // Act
            _userCont.getUserById(user.id)
                .catch(res => {
                    // Assert
                    expect(res).to.be.an.instanceof(APIError);
                    expect(res.message).to.be.equal("Call to DB failed");

                    done();
                });
        }));
    });

    describe("getUserPostsById", () => {
        it("Success", sinon.test(function (done: any): any {
            // Assemble
            let user: User = TestObjects.user;
            let userPost: UserPost = TestObjects.userPost;
            this.stub(requestPromise, "get")
                .returns(Promise.resolve<Array<UserPost>>([userPost]));

            // Act
            _userCont.getUserPostsById(user.id)
                .then(res => {
                    // Assert
                    expect(res).to.be.an.instanceof(Array);
                    expect(res.length).to.be.equal(1);
                    expect(res[0].body).to.be.equal(userPost.body);
                    expect(res[0].id).to.be.equal(userPost.id);
                    expect(res[0].title).to.be.equal(userPost.title);
                    expect(res[0].user_id).to.be.equal(userPost.user_id);

                    done();
                });
        }));

        it("nullId_Throws", sinon.test(function (done: any): any {
            // Assemble
            this.stub(requestPromise, "get")
                .returns(Promise.resolve<Array<UserPost>>(null));

            // Act
            _userCont.getUserPostsById(null)
                .catch(res => {
                    // Assert
                    expect(res).to.be.an.instanceof(APIError);
                    expect(res.status).to.be.eql(httpStatus.BAD_REQUEST);
                    expect(res.message).to.be.eql("ID is required");

                    done();
                });
        }));

        it("callFails_Throws", sinon.test(function (done: any): any {
            // Assemble
            let user: User = TestObjects.user;
            this.stub(requestPromise, "get")
                .throws(new APIError("Random error happened"));

            // Act
            _userCont.getUserPostsById(user.id)
                .catch(res => {
                    // Assert
                    expect(res).to.be.an.instanceof(APIError);
                    expect(res.message.toString()).to.be.equal("APIError: Random error happened");

                    done();
                });
        }));
    });

    describe("createUser", () => {
        it("Success", sinon.test(function (done: any): any {
            // Assemble
            let user: User = TestObjects.user;
            this.stub(MongooseRepository.prototype, "create")
                .returns(Promise.resolve<Array<User>>([user]));

            // Act
            _userCont.createUser(user)
                .then(res => {
                    // Assert
                    expect(res.username).to.be.eql(user.username);
                    expect(res.mobile_number).to.be.eql(user.mobile_number);

                    done();
                });
        }));

        it("errorSaving_Throws", sinon.test(function (done: any): any {
            // Assemble
            this.stub(MongooseRepository.prototype, "create")
                .returns(Promise.reject(new Error("Unable to save user")));

            // Act
            _userCont.createUser(new User())
                .catch(res => {
                    // Assert
                    expect(res).to.be.an.instanceof(APIError);
                    expect(res.message.toString()).to.be.equal("Error: Unable to save user");

                    done();
                });
        }));
    });

    describe("update", () => {
        it("Success", sinon.test(function (done: any): any {
            // Assemble
            let user: User = TestObjects.user;
            this.stub(MongooseRepository.prototype, "update")
                .returns(Promise.resolve<Array<User>>([user]));

            // Act
            _userCont.updateUser(user)
                .then(res => {
                    // Assert
                    expect(res.username).to.be.eql(user.username);
                    expect(res.mobile_number).to.be.eql(user.mobile_number);

                    done();
                });
        }));

        it("randomError_Throws", sinon.test(function (done: any): any {
            // Assemble
            this.stub(MongooseRepository.prototype, "update")
                .throws(new APIError("Random error happened"));

            // Act
            _userCont.updateUser(TestObjects.user)
                .catch(res => {
                    // Assert
                    expect(res).to.be.an.instanceof(APIError);
                    expect(res.message.toString()).to.be.equal("APIError: Random error happened");

                    done();
                });
        }));
    });

    describe("listUsers", () => {
        it("Success", sinon.test(function (done: any): any {
            // Assemble
            let userDoc1: IUserDocument = TestObjects.mongooseUser;
            let userDoc2: IUserDocument = TestObjects.mongooseUser;
            this.stub(mongoose.Query.prototype, "exec")
                .returns(Promise.resolve([userDoc1, userDoc2]));

            // Act
            _userCont.listUsers()
                .then(res => {
                    // Assert
                    expect(res).to.be.an.instanceof(Array);
                    expect(res.length).to.be.eql(2);
                    expect(res[0].username).to.be.eql(userDoc1.username);
                    expect(res[0].mobile_number).to.be.eql(userDoc1.mobile_number);
                    expect(res[1].username).to.be.eql(userDoc2.username);
                    expect(res[1].mobile_number).to.be.eql(userDoc2.mobile_number);

                    done();
                });
        }));

        it("errorListing_Throws", sinon.test(function (done: any): any {
            // Assemble
            this.stub(mongoose.Query.prototype, "exec")
                .returns(Promise.reject(new Error("Unable to list users")));

            // Act
            _userCont.listUsers()
                .catch(res => {
                    // Assert
                    expect(res).to.be.an.instanceof(APIError);
                    expect(res.message.toString()).to.be.equal("Error: Unable to list users");

                    done();
                });
        }));
    });

    describe("remove", () => {
        it("Success", sinon.test(function (done: any): any {
            // Assemble
            let user: User = TestObjects.user;
            this.stub(MongooseRepository.prototype, "delete")
                .returns(Promise.resolve<Array<string>>([user.id]));

            // Act
            _userCont.deleteUser(user.id)
                .then(res => {
                    // Assert
                    expect(res).to.be.eql(user.id);

                    done();
                });
        }));

        it("errorRemoving_Throws", sinon.test(function (done: any): any {
            // Assemble
            this.stub(mongoose.Query.prototype, "exec")
                .returns(Promise.reject(new Error("Unable to remove user")));

            // Act
            _userCont.deleteUser(TestObjects.user.id)
                .catch(res => {
                    // Assert
                    expect(res).to.be.an.instanceof(APIError);
                    expect(res.message.toString()).to.be.equal("Error: Unable to remove user");

                    done();
                });
        }));
    });
});
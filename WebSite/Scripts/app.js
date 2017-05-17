angular.module('auction.services', []);
angular.module('auction.controllers', ['auction.services']);
angular.module('profileApp', [
    'ngRoute',
    'customTemplates',
    'auction.controllers'
])
    .provider('locationPath', function () {
        var locations;

        return {
            setLocations: function (locs) {
                locations = locs;
            },
            $get: function () {
                return locations;
            }
        };
    })
    .config([
        "$routeProvider", "$httpProvider", "$locationProvider", "locationPathProvider", "$compileProvider", "menuItemsProvider",
        function ($routeProvider, $httpProvider, $locationProvider, locationPathProvider, $compileProvider, menuItemsProvider) {
            //If you wish to debug an application with this information then
            //you should open up a debug console in the browser then call this method
            //directly in this console:
            // angular.reloadWithDebugInfo();
            $compileProvider.debugInfoEnabled(false);

            $httpProvider.useApplyAsync(true);

            var tokenService,
                sharedService,
                accessService,
                localizationService,
                locations = {
                    Position: { path: getExternalPath('Position') },
                    Assignment: { path: getExternalPath('Assignment') },
                    Main: { path: getExternalPath('Main') },
                    Application: {
                        pathForPosition: getExternalPath('Application/Position'),
                        pathForAssignment: getExternalPath('Application/Assignment'),
                        pathForCompanyApp: getExternalPath('partner-company/applications')
                    },
                    ApplicationDetails: {
                        pathForPosition: getExternalPath('Application/:applicationId/Position'),
                        pathForAllocation: getExternalPath('Application/:applicationId/Allocation'),
                        getPathForPositionWithId: function (applicationId) { return getExternalPath('Application/' + applicationId + '/Position'); },
                        getPathForAllocationWithId: function (applicationId) { return getExternalPath('Application/' + applicationId + '/Allocation'); }
                    },
                    AssignmentDetails: {
                        path: getExternalPath('Assignment/:allocationId/:applyMode?'),
                        getPathWithId: function (assignmentId) { return getExternalPath('Assignment/' + assignmentId); },
                        getPathWithIdAsViewName: function (assignmentId) { return 'Assignment/' + assignmentId; },
                        getApplyPathWithId: function (assignmentId) { return getExternalPath('Assignment/' + assignmentId + '/Apply'); },
                        getQuickApplyPathWithId: function (assignmentId) { return getExternalPath('Assignment/' + assignmentId + '/QuickApply'); }
                    },
                    PositionDetails: {
                        path: getExternalPath('PositionDetails/:positionId/:applyMode?'),
                        getPathWithId: function (positionId) { return getExternalPath('PositionDetails/' + positionId); },
                        getPathWithIdAsViewName: function (positionId) { return 'PositionDetails/' + positionId; },
                        getApplyPathWithId: function (positionId) { return getExternalPath('PositionDetails/' + positionId + '/Apply'); },
                        getQuickApplyPathWithId: function (positionId) { return getExternalPath('PositionDetails/' + positionId + '/QuickApply'); }
                    },
                    Login: {
                        path: getExternalPath('Login'),
                        getPathWithGroupAndViewName: function (group, viewName) {
                            return 'login?group=' + group + '&viewname=' + viewName;
                        }
                    },
                    Profile: {
                        path: getExternalPath('Profile/:profileSection?'),
                        getDefaultPath: function () { return getExternalPath('Profile'); },
                        getPathWithSelection: function (selection) { return getExternalPath('Profile/' + selection); },
                        profileViewReadOnly: getExternalPath("Partial?viewName=Profile/ProfileViewReadOnly"),
                        profileViewEdit: getExternalPath("Partial?viewName=Profile/ProfileViewEdit"),
                        ProfileSections: {
                            introduction: {
                                view: getExternalPath("Partial?viewName=Profile/Introduction/IntroductionGeneralView")
                            },
                            attachments: {
                                view: getExternalPath("Partial?viewName=Profile/Attachments/AttachmentsView")
                            },
                            languages: {
                                view: getExternalPath("Partial?viewName=Profile/Language/LanguageCommonView"),
                                edit: getExternalPath("Partial?viewName=Profile/Language/LanguageEditView")
                            },
                            basicinformation: {
                                view: getExternalPath("Partial?viewName=Profile/BasicInformation/BasicInformationCommonView"),
                                read: getExternalPath("Partial?viewName=Profile/BasicInformation/BasicInformationReadView"),
                                edit: getExternalPath("Partial?viewName=Profile/BasicInformation/BasicInformationEditView")
                            },
                            competences: {
                                view: getExternalPath("Partial?viewName=Profile/Competence/CompetencesCommonView"),
                                edit: getExternalPath("Partial?viewName=Profile/Competence/CompetencesEditView")
                            },
                            courses: {
                                view: getExternalPath("Partial?viewName=Profile/Course/CourseCommonView"),
                                edit: getExternalPath("Partial?viewName=Profile/Course/CourseEditView")
                            },
                            education: {
                                view: getExternalPath("Partial?viewName=Profile/Education/EducationCommonView"),
                                edit: getExternalPath("Partial?viewName=Profile/Education/EducationEditView")
                            },
                            generalexperience: {
                                view: getExternalPath("Partial?viewName=Profile/Experience/GeneralExperienceView")
                            },
                            certificates: {
                                view: getExternalPath("Partial?viewName=Profile/Certificates/CertificatesCommonView")
                            },
                            experience: {
                                view: getExternalPath("Partial?viewName=Profile/Experience/Experience/ExperienceCommonView"),
                                edit: getExternalPath("Partial?viewName=Profile/Experience/Experience/ExperienceEditView")
                            },
                            employment: {
                                view: getExternalPath("Partial?viewName=Profile/Experience/Employment/EmploymentCommonView"),
                                edit: getExternalPath("Partial?viewName=Profile/Experience/Employment/EmploymentEditView")
                            },
                            industry: {
                                view: getExternalPath("Partial?viewName=Profile/Experience/Industry/IndustryCommonView"),
                                edit: getExternalPath("Partial?viewName=Profile/Experience/Industry/IndustryEditView")
                            },
                            customer: {
                                view: getExternalPath("Partial?viewName=Profile/Experience/Customer/CustomerCommonView"),
                                edit: getExternalPath("Partial?viewName=Profile/Experience/Customer/CustomerEditView")
                            },
                            publications: {
                                view: getExternalPath("Partial?viewName=Profile/Publication/PublicationCommonView"),
                                edit: getExternalPath("Partial?viewName=Profile/Publication/PublicationEditView")
                            }
                        }
                    },
                    PartnerCompany: {
                        path: getExternalPath('partner-company/:section?'),
                        getDefaultPath: function () { return getExternalPath('partner-company'); },
                        getPathWithSelection: function (selection) { return getExternalPath('partner-company/' + selection); }
                    },
                    ConsultantDetails: {
                        path: getExternalPath("consultants/:id/:section?"),
                        getPathWithId: function (id) {
                            return getExternalPath('consultants/' + id);
                        },
                        getPathWithIdForSelection: function (id, selection) {
                            return getExternalPath('consultants/' + id + "/" + selection);
                        }
                    },
                    Subscription: {
                        path: getExternalPath('Subscription/:subscriptionType?'),
                        defaultPath: getExternalPath('Subscription'),
                        getPathWithType: function (subscriptionType) { return getExternalPath('Subscription/' + subscriptionType); }
                    },
                    Error: {
                        getNotFoundPath: function () { return getExternalPath('main/error/notfound'); },
                        getBadRequestPath: function () { return getExternalPath('main/error/badrequest'); },
                        getForbiddenPath: function () { return getExternalPath('main/error/forbidden'); }
                    },
                    HintExample: {
                        path: function () { return getExternalPath('Partial?viewName=HintsExamples/Index'); }
                    }
                },
                tryGetToken = function () {
                    return $.cookie(window.externalWeb.authExistsTokenCookieName);
                },
                getDefaultUrl = function () {
                    var currentUser = sharedService.getUserInformation(),
                        defaultPage,
                        defaultLocations;
                    if (currentUser && currentUser.profileId) {
                        defaultPage = sharedService && accessService &&
                            (!currentUser.partnerCompanyProfileId ||
                            (currentUser.partnerCompanyProfileId && accessService.isConsultant()))
                            ? 'profile'
                            : 'partnerCompany';
                        defaultLocations = {
                            profile: locations.Profile.getDefaultPath(),
                            partnerCompany: locations.PartnerCompany.getDefaultPath()
                        };
                        return defaultLocations[defaultPage] || "";
                    }
                    return 'login-continue';
                },
                menuItems = menuItemsProvider.menuItems;

            locationPathProvider.setLocations(locations);

            delete $httpProvider.defaults.headers.common['X-Requested-With'];

            $httpProvider.interceptors.push(function ($injector) {
                return {
                    'request': function (config) {
                        if (!tokenService) {
                            tokenService = $injector.get("tokenService");
                        }

                        if (!sharedService) {
                            sharedService = $injector.get("sharedService");
                        }

                        if (!accessService) {
                            accessService = $injector.get("accessService");
                        }

                        if (!localizationService) {
                            localizationService = $injector.get("localizationService");
                        }

                        if (config.url.toLowerCase().indexOf("partial?viewname") !== -1) {
                            //hack for caching
                            config.url += "&culture=" + localizationService.getCurrentCulture();
                        }

                        if (!config.headers.CultureCookieName) {
                            config.headers.CultureCookieName = window.externalWeb.cultureCookieName;
                        }

                        return config;
                    },

                    'response': function (response) {
                        return response;
                    },

                    'responseError': function (response) {
                        var q = $injector.get("$q");

                        if (response.status === 503) {
                            window.location.reload();
                        }

                        return q.reject(response);
                    }
                };
            });

            $routeProvider.caseInsensitiveMatch = true;
            $routeProvider
                .when(window.externalWeb.virtualPath, {
                    redirectTo: function () {
                        if (tryGetToken()) {
                            return getDefaultUrl();
                        }
                    }
                })
                .when(locations.Position.path, {
                    templateUrl: window.externalWeb.virtualPath + 'Partial?viewName=Position/Index',
                    controller: 'PositionController',
                    title: window.profileUserInterface.titleExtPositions,
                    menuItem: menuItems.Position
                })
                .when(locations.Assignment.path, {
                    templateUrl: window.externalWeb.virtualPath + 'Partial?viewName=Assignment/Index',
                    controller: 'AssignmentController',
                    title: window.profileUserInterface.titleExtAssignment,
                    menuItem: menuItems.Assignment
                })
                .when(locations.Main.path, {
                    templateUrl: window.externalWeb.virtualPath + 'Partial?viewName=Index',
                    controller: 'MainController',
                    title: window.profileUserInterface.titleExtMain
                })
                .when(locations.Application.pathForPosition, {
                    templateUrl: window.externalWeb.virtualPath + 'Partial?viewName=Application/ApplicationForPosition',
                    controller: 'ApplicationController',
                    title: window.profileUserInterface.titleExtApplication,
                    menuItem: menuItems.MyApplication
                })
                .when(locations.Application.pathForAssignment, {
                    templateUrl: window.externalWeb.virtualPath + 'Partial?viewName=Application/ApplicationForAllocation',
                    controller: 'ApplicationController',
                    title: window.profileUserInterface.titleExtApplication,
                    menuItem: menuItems.MyApplication
                })
                .when(locations.Application.pathForCompanyApp, {
                    templateUrl: window.externalWeb.virtualPath + 'Partial?viewName=Application/ApplicationForAllocation',
                    controller: 'ApplicationController',
                    title: window.profileUserInterface.titleExtApplication,
                    menuItem: menuItems.MyApplication
                })
                .when(locations.ApplicationDetails.pathForPosition, {
                    templateUrl: window.externalWeb.virtualPath + 'Partial?viewName=Application/ApplicationDetailsForPosition',
                    controller: 'ApplicationDetailsForPositionController',
                    title: window.profileUserInterface.titleExtApplicationDetails,
                    menuItem: menuItems.MyApplication
                })
                .when(locations.ApplicationDetails.pathForAllocation, {
                    templateUrl: window.externalWeb.virtualPath + 'Partial?viewName=Application/ApplicationDetailsForAllocation',
                    controller: 'ApplicationDetailsForAllocationController',
                    title: window.profileUserInterface.titleExtApplicationDetails,
                    menuItem: menuItems.MyApplication
                })
                .when(locations.PositionDetails.path, {
                    templateUrl: window.externalWeb.virtualPath + 'Partial?viewName=Position/PositionDetailsView',
                    controller: 'PositionDetailsController',
                    title: window.profileUserInterface.titleExtPositionDetails,
                    menuItem: menuItems.Position
                })
                .when(locations.AssignmentDetails.path, {
                    templateUrl: window.externalWeb.virtualPath + 'Partial?viewName=Assignment/AssignmentDetails',
                    controller: 'AssignmentDetailsController',
                    title: window.profileUserInterface.titleExtAssignment,
                    menuItem: menuItems.Assignment
                })
                .when(locations.Subscription.path, {
                    templateUrl: window.externalWeb.virtualPath + 'Partial?viewName=Subscription/Index',
                    controller: 'SubscriptionController',
                    title: window.profileUserInterface.titleExtSubscription,
                    menuItem: menuItems.Subscription
                })
                .when(locations.Profile.path, {
                    templateUrl: window.externalWeb.virtualPath + 'Partial?viewName=Profile/Index',
                    controller: 'ProfileController',
                    title: window.profileUserInterface.titleExtProfile,
                    menuItem: menuItems.Profile,
                    resolve: ["$q", function ($q) {
                        //prevent to run profileController
                        if (sharedService && sharedService.getUserInformation().partnerCompanyProfileId
                                && accessService && !accessService.isConsultant()) {
                            return $q.defer().promise;
                        }
                    }],
                    redirectTo: function () {
                        if (sharedService && sharedService.getUserInformation().partnerCompanyProfileId
                                && accessService && !accessService.isConsultant()) {
                            return locations.PartnerCompany.getDefaultPath();
                        }
                    }
                })
                .when(locations.PartnerCompany.path, {
                    templateUrl: window.externalWeb.virtualPath + 'Partial?viewName=PartnerCompany/Index',
                    controller: 'PartnerCompanyController',
                    title: window.profileUserInterface.titleExtPartnerCompany,
                    menuItem: menuItems.Profile
                })
                .when(locations.ConsultantDetails.path, {
                    templateUrl: window.externalWeb.virtualPath + 'Partial?viewName=PartnerCompany/Consultant/Index',
                    controller: 'ConsultantDetailsController',
                    title: window.profileUserInterface.titleExtPartnerCompany,
                    menuItem: menuItems.Profile
                })
                 .when('login-continue', {
                     title: window.profileUserInterface.titleExtMain
                 })
                .when(getExternalPath('Main/Error/:type'), {
                    templateUrl: function (params) {
                        window.location.href = getExternalPath('Main/Error/' + params.type);
                    }
                })
                .otherwise({
                    redirectTo: function () {
                        if (tryGetToken()) {
                            return getDefaultUrl();
                        }

                        return locations.Error.getNotFoundPath();
                    }
                });

            $locationProvider.html5Mode(true);
        }
    ]).run([
        '$rootScope',
        '$location',
        'editableOptions',
        '$timeout',
        'sharedService',
        'accessService',
        'tokenService',
        'authScopeConsts',
        'storageService',
        'menuItems',
        'localizationService',
        function (
            $rootScope,
            $location,
            editableOptions,
            $timeout,
            sharedService,
            accessService,
            tokenService,
            authScopeConsts,
            storageService,
            menuItems,
            localizationService
        ) {
            console.log("Application was started");

            $rootScope.$profileEnums = window.profileEnums;
            $rootScope.$permissions = window.profileEnums.Permissions;
            $rootScope.$authScopes = authScopeConsts;
            $rootScope.rememberGridRow = sharedService.rememberGridRow;
            $rootScope.clearTableMetadata = function () {
                storageService.removeItemsWhereKeyContainsValue('TableRow_');
                storageService.removeItemsWhereKeyContainsValue('_selectedItems');
            };

            $rootScope.$externalWeb = window.externalWeb;
            $rootScope.$loginWeb = window.loginWeb;
            $rootScope.cprfHelper = {
                keysInObject: window.Object.keys,
                jsonStr: function (obj) { //json stringify with cyclic handling
                    var seen = [];
                    return JSON.stringify(obj, function (key, val) {
                        if (val !== null && typeof val === "object") {
                            if (seen.indexOf(val) >= 0) {
                                return null;
                            }
                            seen.push(val);
                        }
                        return val;
                    }, 4);
                },
                getLocaleText: function (key) {
                    return decodeURIComponent(window.profileUserInterface[key]);
                },
                log: console.log,
                getFlagUrl: localizationService.getFlagUrl,
                oppositeCulture: localizationService.getOppositeCulture
            };

            editableOptions.theme = 'bs3';
            editableOptions.blurElem = "ignore";

            $rootScope.$on("$routeChangeStart", function (event, next) {
                var viewName = $location.search().viewname,
                    redirectPath = next.originalPath,
                    i = 0,
                    token = tokenService.getAuthToken();

                if (viewName) {
                    sharedService.getTokenForView(viewName);
                    $location.url(viewName.toLowerCase());
                    return;
                }

                if ($location.$$url.match("\\?culture\\=*")) {
                    $location.url($location.path());
                    return;
                }

                if (next.keys) {
                    for (i; i < next.keys.length; i++) {
                        redirectPath = redirectPath.replace(next.keys[i].name, next.params[next.keys[i].name]);
                        redirectPath = redirectPath.toLowerCase();
                    }
                }

                if (redirectPath && token && token.isAuthenticated &&
                        !accessService.checkAccessToThePath(redirectPath, next.pathParams)) {
                    window.location.href = window.externalWeb.url + "main/error/forbidden";
                }
            });

            $rootScope.$on('$routeChangeSuccess', function (event, current) {
                var title = "Profiler - " + ((current && current.title) || "");
                sharedService.updateTitleAndSendToGa(null, decodeURIComponent(title));
                menuItems.selectMenuItem(current.menuItem);

                $timeout(function () {
                    $("html, body").animate({ scrollTop: 0 }, '500', 'swing');
                }, 0);
            });

            //fix datatables width
            $(window).on("resize", sharedService.fixTableWidth);

            window.tinyMCE.baseURL = window.externalWeb.url + "Content/Libs/tinymce";
            if (window.PDFJS) {
                window.PDFJS.workerSrc = window.externalWeb.url + "Content/Libs/pdf.js/build/pdf.worker.js";
            }
        }
    ]);
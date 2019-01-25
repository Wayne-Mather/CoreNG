# CORENG README
We use @nrwl/schematics to create our angular applications.

That way we can share services, components, directives between applications in a monolithic reapo.

It also allows us to create apps specifically tailored for various parts of our application. 

## Why do this?
This code base is for Enterprise Applications that consist of many different features and sub-features.

So this is how Areas work in dotnet core - each area is a unique feature in the application with it's own route. We let dotnet core handle the authentication and authorisation and then we can tailor the front-end in each area as sees fit.  This is called a "Hybrid Model"

## Hybrid Model?
Yes - some areas simple razor pages may work and not require a full-blown feature rich SPA - others are so complex that they need to function like a desktop app and having Angular enables us to fullfill those roles. This is where the @nrwl/schematics comes into play - we create a new application for every page that we need to host

# Angular Model
The way we have decided to build the model is via SmartComponents and Presentation Components

## Smart Components
These are the glue - the business logic and the API callers of the application. They should also use the EventBus to send and listen to messages to/from other smart components.

They then interact with the children (presentation) via the standard @Input()/@Output methods.

These should contain the name "Shell" at the end to separate them from presentation components.

They should also use SubSink() for any eventbus "listeners" and implement OnDestory() and call the unsubscribe method.

Please look at the branch Example App to see how to implement a demo application

## Presentation Components
These components just allow the viewing of data or the ability to edit the data in memory - they do not do anything else. They use @Input() and @Output() to talk to the parent smart component to do the required workflows as necessary.

@Input() should be used for all data that the component needs to render
@Output() should be used to signal to the parent that some "process" is ready to be completed.

Best practice would be for the component to have (click) events called onEventName() and the @Output() should be named EventName


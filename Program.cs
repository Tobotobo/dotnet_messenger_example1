using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection()
    .AddTransient<IMessenger>(serviceProvider =>
    {
        var messenger = new StrongReferenceMessenger();
        messenger.Register(new ServiceProviderRequestRecipient(serviceProvider));
        return messenger;
    })
    .BuildServiceProvider();

var messenger1 = services.GetRequiredService<IMessenger>();
var messenger2 = services.GetRequiredService<IMessenger>();

messenger1.Register(new NameRequestRecipient("東京　太郎"));
messenger2.Register(new NameRequestRecipient("東京　花子"));

var name1 = messenger1.Send<NameRequestMessage>().Response;
var name2 = messenger2.Send<NameRequestMessage>().Response;

Console.WriteLine($"name1 = {name1}, name2 = {name2}");


#region NameRequest
class NameRequestMessage : RequestMessage<string> { }

class NameRequestRecipient : IRecipient<NameRequestMessage>
{
    private readonly string _name;

    public NameRequestRecipient(string name)
    {
        _name = name;
    }

    public void Receive(NameRequestMessage msg)
    {
        msg.Reply(_name);
    }
}
#endregion

#region ServiceProviderRequest
class ServiceProviderRequestMessage : RequestMessage<IServiceProvider> { }

class ServiceProviderRequestRecipient : IRecipient<ServiceProviderRequestMessage>
{
    private readonly IServiceProvider _services;

    public ServiceProviderRequestRecipient(IServiceProvider services)
    {
        _services = services;
    }

    public void Receive(ServiceProviderRequestMessage msg)
    {
        msg.Reply(_services);
    }
}
#endregion
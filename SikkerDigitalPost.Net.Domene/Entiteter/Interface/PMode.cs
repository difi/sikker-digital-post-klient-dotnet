namespace SikkerDigitalPost.Net.Domene.Entiteter
{
    public abstract class PMode
    {        
    // ROLES
	public static readonly string RolleMeldingsformidler = "urn:sdp:meldingsformidler";
	public static readonly string RolleAvsender = "urn:sdp:avsender";
	public static readonly string RollePostkasse = "urn:sdp:postkasseleverandør";

	// PARTY IDs
	public static readonly string PartyIdType = "urn:oasis:names:tc:ebcore:partyid-type:iso6523:9908";

	// COLLABORATION INFO
	public static readonly string FormidlingAgreementRefOld = "http://begrep.difi.no/SikkerDigitalPost/Meldingsutveksling/FormidleDigitalPostForsendelse";
	public static readonly string FormidlingAgreementRef = "http://begrep.difi.no/SikkerDigitalPost/1.0/transportlag/Meldingsutveksling/FormidleDigitalPostForsendelse";
	public static readonly string FlyttAgreementRef = "http://begrep.difi.no/SikkerDigitalPost/1.0/transportlag/Meldingsutveksling/FlyttetDigitalPost";
	public static readonly string Service = "SDP";

    }
}

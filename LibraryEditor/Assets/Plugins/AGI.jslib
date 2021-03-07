mergeInto(LibraryManager.library, {
  
  // Get information about the player (if AGI is connected)
  // SendMessage calls are all bespoke/optional examples of use.
  GetUser: function () {
    if (typeof this.ag !== 'undefined') {   // Useful line to see if connected to AGI
      this.ag.authenticateUser().then(function(user) {
        this.user = user;
        SendMessage("AGI", "SetUsername", this.user.username);
        SendMessage("AGI", "SetUID", this.user.uid);
        SendMessage("AGI", "SetAvatar", this.user.avatar);
      }).catch(function(err) {
        console.error("Error: " + err.message);
      });
    }
  },

  // Opens a new tab in browser
  OpenPage: function (url) {
   url = Pointer_stringify(url);
   console.log('Opening link: ' + url);
   window.open(url,'_blank');
  	},

  // Not used anywhere but an example of checking if the user is authenticated.
  // Devs might want to use this in Unity before submitting things.
  // Returns true if authenticated, false if not.
  HasAGI: function () {
    if (typeof this.ag !== 'undefined') {
      return true;
    }
    return false;
  },

  // The main connection function.
  // Sets the document's domain, which is required for services.
  // Creates and stores the main ArmorGames object used for all other calls.
  // Also sets up the store listener, which handles all purchases.
  // Returns true to Unity if successful.
  // MAKE SURE YOU INSERT YOUR "api_key"
  GetAGI: function () {
    document.domain = "armorgames.com";
    try {
      if (typeof parent.agi !== 'undefined') {
        this.ag = new ArmorGames({
          user_id: parent.apiAuth.user_id,
          auth_token: parent.apiAuth.auth_token,
          game_id: parent.apiAuth.game_id,
          api_key: 'DCEFD58E-AB2E-45DD-ABD2-D88E8B1A0FD9',
          agi: parent.agi
        });
        // Store listener
        this.ag.setPurchaseHandler(function(event, eventName, payload) {
          if (eventName == "completePurchase") {
            var details;
            if (payload.is_consumable == false) {
              details = payload.sku + "&true";
              SendMessage("AGI", "RetrievedUnlockable", details);
            } else {
              details = payload.sku + "&" + payload.purchase_quantity;
              SendMessage("AGI", "PurchasedConsumable", details);
            }
          } else {
           // If needed, you can respond to the storefront closing without a purchase.
          }
        });
        // End store listener
        return true;
      }
    } catch(err) {
      window.alert("Error: " + err.message);
    }
    return false;
  },

  // Example save function. A single integer value is used here.
  // Saves the data (d) against the key (k)
  // Uses 2 example functions in Unity to feed data back to the game.
  SaveGame: function(k, d) {
    var keystr = Pointer_stringify(k);
    this.ag.saveGame(keystr, d).then(function(response) {
      if (response.success == 1) {
        SendMessage("AGI", "SaveSuccessful", keystr);
      } else {
        SendMessage("AGI", "SaveFailed", keystr);
      }
    }).catch(function(err) {
      console.error("Error: " + err.message);
    });
  },

  // Example load function. Loads our integer value from above
  // Attempts to load from a save with the named key (k)
  // Triggers the "SaveLoaded" function in Unity
  // String sent back to Unity includes key and the value
  LoadGame: function(k) {
    var keystr = Pointer_stringify(k);
    this.ag.retrieveGame(keystr).then(function(response) {
      var details = keystr + "&" + response[keystr];
      SendMessage("AGI", "SaveLoaded", details);
    }).catch(function(err) {
      console.error("Error: " + err.message);
    });
  },

  // Erase a save with the provided key (k)
  EraseGame: function(k) {
    var keystr = Pointer_stringify(k);
    this.ag.eraseGame(keystr).then(function(response) {
      if (response[keystr] == "deleted") {
        SendMessage("AGI", "EraseSuccessful", keystr);
      } else {
        SendMessage("AGI", "EraseFailed", keystr);
      }
    }).catch(function(err) {
      console.error("Error: " + err.message);
    });
  },

  // Increment a numerical save with the key (k) by amount (d)
  IncrementGame: function(k, d) {
    var keystr = Pointer_stringify(k);
    this.ag.incrementGame(keystr, d).then(function(response) {
      if (typeof response[keystr] !== 'undefined') {
        SendMessage("AGI", "IncrementSuccessful", keystr + "&" + response[keystr]);
      } else {
        SendMessage("AGI", "IncrementFailed", keystr);
      }
    }).catch(function(err) {
      console.error("Error: " + err.message);
    });
  },

  // Decrement a numerical save with the key (k) by amount (d)
  DecrementGame: function(k, d) {
    var keystr = Pointer_stringify(k);
    this.ag.decrementGame(keystr, d).then(function(response) {
      if (typeof response[keystr] !== 'undefined') {
        SendMessage("AGI", "DecrementSuccessful", keystr + "&" + response[keystr]);
      } else {
        SendMessage("AGI", "DecrementFailed", keystr);
      }
    }).catch(function(err) {
      console.error("Error: " + err.message);
    });
  },

  // Returns a list of all quest progress. Look at AGI doc for more info
  GetQuests: function() {
    this.ag.retrieveQuests().then(function(response) {
      if (response.length == 0) {
        SendMessage("AGI", "NoQuestInfo");
      }
      for(var index in response) {
        var details = response[index].developer_id + "&" + response[index]. current_value;
        SendMessage("AGI", "QuestRetrieved", details);
      }
    }).catch(function(err) {
      console.error("Error: " + err.message);
    });
  },

  // Submit progress (progress) against a quest id (id)
  SubmitQuest: function(id, progress) {
    var idstr = Pointer_stringify(id);
    window.alert("Submitting " + idstr + " as " + progress);
    this.ag.submitQuest(idstr, progress).then(function(response) {
      SendMessage("AGI", "QuestSubmitSuccessful", response.developer_id + "&" + response.current_value);
    });
  },

  // Resets the quest with the given id
  ResetQuest: function(id) {
    var idstr = Pointer_stringify(id);
    this.ag.resetQuest(idstr).then(function(response) {
      SendMessage("AGI", "QuestResetSuccessful", response.developer_id);
    });
  },

  // Shows all purchase options for this game
  ShowStorefront: function(sku) {
  	var skustr = Pointer_stringify(sku);
  	if(skustr===""){
  		this.ag.showStorefront();
  	}else{
  		this.ag.showStorefront(skustr);
  	}
    
  },

  // Gets a list of a user's current purchases.
  // Example feeds back to Unity differently for unlockable and consumables
  RetrievePurchases: function() {
    this.ag.retrievePurchases().then(function(response) {
      for(var index in response) {
        var details;
        if (response[index].product.type == "unlockable") {
          details = response[index].product.sku + "&" + response[index].purchase.success;
          SendMessage("AGI", "RetrievedUnlockable", details);
        } else if (response[index].product.type == "consumable") {
          details = response[index].product.sku + "&" + response[index].purchase.data;
          SendMessage("AGI", "RetrievedConsumable", details);
        }
      }
    });
  },

  // Tries to consume an amount (q) of a consumable sku (k)
  // Consuming at 0 causes a console error but works fine in Unity.
  // Returns the amount of consumables remaining after successful consumption.
  ConsumePurchase: function(k, q) {
    var keystr = Pointer_stringify(k);
    this.ag.consume(keystr, q).then(function(response) {
    // This may be updated by Armor to return an object even when failing.
    // Currently makes a console error if there's nothing to consume.
      if (response == false) {
        SendMessage("AGI", "ConsumeFailed", keystr);
      } else {
        SendMessage("AGI", "ConsumeSuccessful", keystr + "&" + response.purchase.data);
      }
    });
  }

});